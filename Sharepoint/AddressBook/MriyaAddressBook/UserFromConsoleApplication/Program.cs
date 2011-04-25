///////////////////////////////////////////////////////////////////////////////
// 
// This application is designed to retreive employees profiles from 1C
// and update SP profiles at Mriya
// 
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Drawing;
using System.IO;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Portal;
using Microsoft.SharePoint.Portal.WebControls;
using Microsoft.Office.Server;
using Microsoft.Office.Server.UserProfiles;
using UserProfilesDAL;

namespace UserFromConsoleApplication
{
    /// <summary>
    /// Reads records from 1C through the SOAP web service and updates SharePoint accounts
    /// Check application settings config file to set upa correct SOAP and SP urls, to 
    /// enable and disable fields to be updated
    /// </summary>
    class Program
    {
        TableProfiles usersExisting = new TableProfiles();
        TableProfiles users1C = new TableProfiles();
        SPProfilePhotoUploader imageUploader = null;
        int nAccount = 0;

        static void Main(string[] args)
        {
            Program program = new Program();

            Console.WriteLine("Reading 1C profiles, please wait...");
            if (program.Read1CRecords())
            {
                Console.WriteLine("Imported {0} records.", program.users1C.Count);
                Console.WriteLine("Reading SP profiles, please wait...");
                if (program.ReadSPRecords())
                {
                    Console.WriteLine("Imported {0} records.", program.usersExisting.Count);
                    Console.WriteLine("Updating SP profiles, please wait...");
                    if (program.UpdateProfiles())
                    {
                        Console.WriteLine("Done.");
                    }
                }
            }
            ReadKey();
        }

        /// <summary>
        /// Optional (Debug only). Reads any key after records were updated.
        /// </summary>
        [Conditional("DEBUG")]
        static void ReadKey()
        {
            Console.WriteLine("\n\nPress any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Finds SP UserProfile by: Last Name, First Name, Middle Nam and Birthday
        /// </summary>
        /// <param name="profile">RecordUserProfile record wich is imported from 1C</param>
        /// <returns>Returns RecordUserProfile (which is imported from SP) if found, otherwise returns null</returns>
        RecordUserProfile GetProfileByPersonName(RecordUserProfile profile)
        {
            foreach (RecordUserProfile profileRecord in usersExisting.GetProfiles())
            {
                if (profileRecord.FirstName.Trim().ToLower() == profile.FirstName.Trim().ToLower()
                    && profileRecord.MiddleName.Trim().ToLower() == profile.MiddleName.Trim().ToLower()
                    && profileRecord.LastName.Trim().ToLower() == profile.LastName.Trim().ToLower()
                    && DateTime.Compare(profileRecord.BirthdayDT, profile.BirthdayDT) == 0)
                    return profileRecord;
            }
            return null;
        }

        /// <summary>
        /// Finds SP UserProfile by INN
        /// </summary>
        /// <param name="profile">RecordUserProfile record wich is imported from 1C</param>
        /// <returns>Returns RecordUserProfile (which is imported from SP) if found, otherwise returns null</returns>
        RecordUserProfile GetProfileByINN(RecordUserProfile profile)
        {
            string sINN = profile.INN.Trim().ToLower();
            foreach (RecordUserProfile profileRecord in usersExisting.GetProfiles())
            {
                if (profileRecord.INN.Trim().ToLower() == sINN)
                    return profileRecord;
            }
            return null;
        }

        /// <summary>
        /// Imports records (list of RecordUserProfile) from 1C through SOAP
        /// </summary>
        /// <returns>true on success, false on failed</returns>
        bool Read1CRecords()
        {
            try
            {
                users1C = Import1CUserProfiles.ImportRecords(Properties.Settings.Default.soapServerUrl,
                    Properties.Settings.Default.soapServerUser,
                    Properties.Settings.Default.soapServerPassword);
            }
            catch (Exception ex)
            {
                string sError = string.Format("Read1CRecords(): Error was occured while reading 1C user profile records:\n{0}",
                    ex.ToString());

                TextWriter errorWriter = Console.Error;
                Console.WriteLine(sError);
                errorWriter.WriteLine(sError);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Imports records (list of RecordUserProfile) from SharePoint site
        /// </summary>
        /// <returns>true on success, false on failed</returns>
        bool ReadSPRecords()
        {
            try
            {
                string strSiteURL = Properties.Settings.Default.sharePointUrl;
                using (SPSite site2 = new SPSite(strSiteURL))
                {
                    using (SPWeb web = site2.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        SPServiceContext sc = SPServiceContext.GetContext(site2);
                        UserProfileManager upm = new UserProfileManager(sc);
                        foreach (UserProfile profile in upm)
                        {
                            // TODO: Figure out how to filter the list and skill all service accounts
                            if (profile["AccountName"].Value != null &&
                                (profile["AccountName"].Value.ToString().ToUpper().IndexOf("\\SM_") >= 0 ||
                                profile["AccountName"].Value.ToString().ToUpper().IndexOf("\\SP_") >= 0))
                                continue;
                            if (profile["AccountName"].Value != null &&
                                profile["AccountName"].Value.ToString() == profile.DisplayName)
                                continue;

                            usersExisting.Add(profile);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string sError = string.Format("ReadSPRecords(): Error was occured while reading user profiles:\n{0}",
                    ex.ToString());

                TextWriter errorWriter = Console.Error;
                Console.WriteLine(sError);
                errorWriter.WriteLine(sError);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Writes SharePoint UserProfile property value
        /// </summary>
        /// <param name="profile">reference to the UserProfile object instance</param>
        /// <param name="strField">property name</param>
        /// <param name="objValue">property value</param>
        /// <returns>true on success, false on failed</returns>
        bool WriteSPProfileField(ref UserProfile profile, string strField, object objValue)
        {
            try
            {
                profile[strField].Value = objValue;
            }
            catch (Exception e)
            {
                string sError = string.Format("WriteSPProfileField(): Error was occured while updating user profile \"{0}\" field:\n{1}",
                    strField, e.Message);

                TextWriter errorWriter = Console.Error;
                Console.WriteLine(sError);
                errorWriter.WriteLine(sError);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Updates SharePoint UserProfile property value. It updates if existing profile property
        /// is not equal to the new value
        /// </summary>
        /// <param name="profile">reference to the UserProfile object instance</param>
        /// <param name="strField">property name</param>
        /// <param name="objValue">property value</param>
        /// <returns>true on success, false on failed</returns>
        bool UpdateSPProfileField(ref UserProfile profile, string strField, object objValue)
        {
            try
            {
                if (profile[strField].Value == null || profile[strField].Value != objValue)
                    profile[strField].Value = objValue;
            }
            catch (Exception e)
            {
                string sError = string.Format("UpdateSPProfileField(): Error was occured while updating user profile \"{0}\" field:\n{1}",
                    strField, e.Message);

                TextWriter errorWriter = Console.Error;
                Console.WriteLine(sError);
                errorWriter.WriteLine(sError);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Updates SharePoint UserProfile property string value. It updates if existing profile property
        /// is not equal to the new value
        /// </summary>
        /// <param name="profile">reference to the UserProfile object instance</param>
        /// <param name="strField">property name</param>
        /// <param name="strValue">property value</param>
        /// <returns>true on success, false on failed</returns>
        bool UpdateSPStringProfileField(ref UserProfile profile, string strField, string strValue)
        {
            if (strValue == null)
                strValue = "";
            try
            {
                if (profile[strField].Value == null || 
                    profile[strField].Value.ToString().Trim().ToLower().CompareTo(strValue.Trim().ToLower()) != 0)
                    profile[strField].Value = strValue;
            }
            catch (Exception e)
            {
                string sError = string.Format("UpdateSPStringProfileField(): Error was occured while updating user profile \"{0}\" field:\n{1}",
                    strField, e.Message);

                TextWriter errorWriter = Console.Error;
                Console.WriteLine(sError);
                errorWriter.WriteLine(sError);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Creates new SharePoint user profile record
        /// </summary>
        /// <param name="site">SPSite object</param>
        /// <param name="upm">UserProfileManager object</param>
        /// <param name="profile">Profile record (imported from 1C) to be added</param>
        /// <param name="subType">Type of UserProfile</param>
        /// <returns>true on success, false on failed</returns>
        bool CreateUserProfile(SPSite site, UserProfileManager upm, RecordUserProfile profile, ProfileSubtype subType)
        {
            UserProfile p = null;
            string strAccountName = "";
            string strDisplayName = "";
            
            try
            {
                // create a user profile and set properties   
                if (profile.AccountName.Trim().Length > 0)
                    strAccountName = profile.AccountName;
                else
                    strAccountName = string.Format("SqlMembershipProvider:imported1c{0}", ++nAccount);

                strDisplayName = string.Format("{0} {1}{2}",
                    profile.FirstName.Trim(), (profile.MiddleName.Trim().Length > 0) ? (profile.MiddleName.Trim() + " ") : (""),
                    profile.LastName);

                p = upm.CreateUserProfile(strAccountName);

                p.DisplayName = strDisplayName;
                WriteSPProfileField(ref p, "FirstName", profile.FirstName);
                WriteSPProfileField(ref p, "LastName", profile.LastName);
                WriteSPProfileField(ref p, "SPS-Birthday", profile.Birthday);

                WriteSPProfileField(ref p, "WorkPhone", profile.PhoneWork);
                WriteSPProfileField(ref p, "HomePhone", profile.PhoneHome);
                WriteSPProfileField(ref p, "WorkEmail", profile.EmailWork);
                WriteSPProfileField(ref p, "Office", profile.SeparateDivision);
                WriteSPProfileField(ref p, "Department", profile.SubDivision);
                WriteSPProfileField(ref p, "Title", profile.Position);
                WriteSPProfileField(ref p, "SPS-JobTitle", profile.Position);
                WriteSPProfileField(ref p, "SPS-HireDate", profile.DateOfEmployment);

                // Custom fields
                WriteSPProfileField(ref p, "Mr-MiddleName", profile.MiddleName);
                WriteSPProfileField(ref p, "Mr-Inn", profile.INN);
                WriteSPProfileField(ref p, "Mr-Ssn", profile.SSN);
                WriteSPProfileField(ref p, "Mr-Organization", profile.Organization);

                // Photos
                if (profile.Photo != null)
                {
                    if (imageUploader != null)
                    {
                        imageUploader.UploadPhoto(p, profile.Photo);
                        string pictureUrl = String.Format("{0}/{1}/{2}_MThumb.jpg", site.Url,
                            imageUploader.GetSubfolderName(), imageUploader.GetFileNameFromAccount(p));
                        p["PictureUrl"].Value = pictureUrl;
                    }
                }
                else
                {
                }

                p.ProfileSubtype = subType;

                p.Commit();
            }
            catch (Exception ex)
            {

                string sError = string.Format("CreateUserProfile() The error was occured while updating profile:\nAccount name: {0}\nDisplayName: {1}\nError:\n\n{2}\n---\n",
                    strAccountName, strDisplayName, ex.ToString());

                TextWriter errorWriter = Console.Error;
                Console.WriteLine(sError);
                errorWriter.WriteLine(sError);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Updates existing SharePoint user profile
        /// </summary>
        /// <param name="site">SPSite objec</param>
        /// <param name="p">The existing SP profile to be updated</param>
        /// <param name="profile">Profile record, which is imported from 1C</param>
        /// <returns>true on success, false on failed</returns>
        bool UpdateUserProfile(SPSite site, UserProfile p, RecordUserProfile profile)
        {
            string strDisplayName = "";
            
            try
            {
                strDisplayName = string.Format("{0} {1}{2}",
                    profile.FirstName.Trim(), (profile.MiddleName.Trim().Length > 0) ? (profile.MiddleName.Trim() + " ") : (""),
                    profile.LastName);

                if (Properties.Settings.Default.updateProfileDisplayName && p.DisplayName != strDisplayName)
                    p.DisplayName = strDisplayName;

                if (Properties.Settings.Default.updateFirstName)
                    UpdateSPStringProfileField(ref p, "FirstName", profile.FirstName);
                if (Properties.Settings.Default.updateLastName)
                    UpdateSPStringProfileField(ref p, "LastName", profile.LastName);
                if (Properties.Settings.Default.updateBirthday)
                    UpdateSPProfileField(ref p, "SPS-Birthday", profile.Birthday);

                if (Properties.Settings.Default.updatePhoneWork)
                    UpdateSPStringProfileField(ref p, "WorkPhone", profile.PhoneWork);
                if (Properties.Settings.Default.updatePhoneHome)
                    UpdateSPStringProfileField(ref p, "HomePhone", profile.PhoneHome);
                if (Properties.Settings.Default.updateEmailWork)
                    UpdateSPStringProfileField(ref p, "WorkEmail", profile.EmailWork);
                if (Properties.Settings.Default.updateSeparateDivision)
                    UpdateSPStringProfileField(ref p, "Office", profile.SeparateDivision);
                if (Properties.Settings.Default.updateSubDivision)
                    UpdateSPStringProfileField(ref p, "Department", profile.SubDivision);
                if (Properties.Settings.Default.updatePosition)
                {
                    UpdateSPStringProfileField(ref p, "Title", profile.Position);
                    UpdateSPStringProfileField(ref p, "SPS-JobTitle", profile.Position);
                }
                if (Properties.Settings.Default.updateDateOfEmployment && profile.EmploymentDate != null)
                    UpdateSPProfileField(ref p, "SPS-HireDate", profile.EmploymentDate);
                
                // Custom fields
                if (Properties.Settings.Default.updateMiddleName)
                    UpdateSPStringProfileField(ref p, "Mr-MiddleName", profile.MiddleName);
                if (Properties.Settings.Default.updateINN)
                    UpdateSPStringProfileField(ref p, "Mr-Inn", profile.INN);
                if (Properties.Settings.Default.updateSSN)
                    UpdateSPStringProfileField(ref p, "Mr-Ssn", profile.SSN);
                if (Properties.Settings.Default.updateOrganization)
                    UpdateSPStringProfileField(ref p, "Mr-Organization", profile.Organization);

                // Photos
                if (Properties.Settings.Default.updatePhoto && profile.Photo != null)
                {
                    if (Properties.Settings.Default.forceUpdatePhoto == true ||
                        p["PictureUrl"].Value == null || p["PictureUrl"].Value.ToString().Length < 1)
                    {
                        if (imageUploader != null)
                        {
                            imageUploader.UploadPhoto(p, profile.Photo);
                            string pictureUrl = String.Format("{0}/{1}/{2}_MThumb.jpg", site.Url,
                                imageUploader.GetSubfolderName(), imageUploader.GetFileNameFromAccount(p));
                            WriteSPProfileField(ref p, "PictureUrl", pictureUrl);
                        }
                    }
                }
                else
                {
                    if (Properties.Settings.Default.forceUpdatePhoto == true &&
                        p["PictureUrl"].Value != null && p["PictureUrl"].Value.ToString().Length > 0)
                    {
                        // TODO: Remove image?
                        WriteSPProfileField(ref p, "PictureUrl", null);
                    }
                }

                p.Commit();
            }
            catch (Exception ex)
            {
                string sError = string.Format("UpdateUserProfile() The error was occured while updating profile:\nAccount name: {0}\nDisplayName: {1}\nError:\n\n{2}\n---\n",
                    (p != null && p["AccountName"].Value != null) ? (" \"" + p["AccountName"].Value.ToString() + "\"") : (""), 
                    strDisplayName, ex.ToString());

                TextWriter errorWriter = Console.Error;
                Console.WriteLine(sError);
                errorWriter.WriteLine(sError);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Enumerates list of imported from 1C profile records and updates SP profiles
        /// </summary>
        /// <returns>true on success, false on failed</returns>
        bool UpdateProfiles()
        {
            int nAdded = 0;
            int nModified = 0;

            try
            {
                string strSiteURL = Properties.Settings.Default.sharePointUserProfilesUrl;
                using (SPSite site = new SPSite(strSiteURL))
                {
                    SPWeb web = site.OpenWeb();
                    SPServiceContext context = SPServiceContext.GetContext(site);
                    UserProfileManager upm = new UserProfileManager(context);
                    ProfileSubtypeManager psm = ProfileSubtypeManager.Get(context);

                    web.AllowUnsafeUpdates = true;

                    // choose default user profile subtype as the subtype
                    string subtypeName = ProfileSubtypeManager.GetDefaultProfileName(ProfileType.User);
                    ProfileSubtype subType = psm.GetProfileSubtype(subtypeName);

                    try
                    {
                        imageUploader = new SPProfilePhotoUploader(web);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("{0}\nImages will not be updated", ex.ToString());
                        imageUploader = null;
                    }

                    double total = users1C.Count;
                    double rec = 100.0 / total;
                    double current = 0.0;
                    
                    Console.Write("Updated 0%");
                    foreach (RecordUserProfile profile in users1C.GetProfiles())
                    {
                        RecordUserProfile profileFound = null;
                        string profileAccountName = "";
                        bool profileExists = false;

                        // If LDAP is refer to account name then verify if such account is existing
                        if (profile.AccountName != null &&
                            profile.AccountName.Trim().Length > 0 && 
                            Properties.Settings.Default.accountNameIsAKey == true)
                        {
                            if (upm.UserExists(profile.AccountName.Trim()))
                            {
                                profileAccountName = profile.AccountName.Trim();
                                profileExists = true;
                            }

                        }
                        // If innIsAKey option is true and account is not found then try to find
                        // profile by INN
                        if (profileExists == false &&
                            profile.INN != null &&
                            profile.INN.Trim().Length > 0 && 
                            Properties.Settings.Default.innIsAKey)
                        {
                            profileFound = GetProfileByINN(profile);
                            if (profileFound != null)
                            {
                                profileAccountName = profileFound.AccountName;
                                profileExists = true;
                            }
                        }
                        // Find by Last name, First name, Middle name (FIO) and Birthday
                        if (profileExists == false)
                        {
                            profileFound = GetProfileByPersonName(profile);
                            if (profileFound != null)
                            {
                                profileAccountName = profileFound.AccountName;
                                profileExists = true;
                            }
                        }

                        if (profileExists)
                        {
                            // get existing profile
                            UserProfile p = upm.GetUserProfile(profileAccountName);

                            if (UpdateUserProfile(site, p, profile))
                                nModified++;
                        }
                        else
                        {
                            // create a user profile and set properties   
                            if (CreateUserProfile(site, upm, profile, subType))
                                nAdded++;
                        }
                        current += rec;
                        Console.Write("\rUpdated {0:F}%", current);
                    }
                    Console.Write("\r");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            Console.WriteLine("Updated {0} records.", nModified);
            Console.WriteLine("Added {0} new records.", nAdded);
            return true;
        }
    }
}
