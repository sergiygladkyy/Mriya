using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace MriyaStaffDAL
{
    [System.ComponentModel.DataObject(true)]
    public class DepartmentTable
    {
        private List<DepartmentRecord> _Records = new List<DepartmentRecord>();
        private bool _bEmpty = true;

        #region Properties

        public bool IsEmpty
        {
            get { return _bEmpty; }
            set { _bEmpty = value; }
        }

        public int Count
        {
            get { return GetCount(); }
        }

        #endregion Properties

        public DepartmentTable()
        {
        }

        public DepartmentTable(SqlConnection connection)
        {
            ReadFromDB(connection);
        }

        public int GetCount()
        {
            return _Records.Count;
        }

        public void Clear()
        {
            _Records.Clear();
            _bEmpty = true;
        }

        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<DepartmentRecord> GetRecords()
        {
            return _Records;
        }

        public void ReadFromDB(SqlConnection connection)
        {
            string sQuery = "SELECT DISTINCT f16_abteilun AS department " +
                "FROM UserFields407 " +
                "WHERE f16_abteilun IS NOT NULL AND f16_abteilun > '' " +
                "ORDER BY f16_abteilun";
            SqlCommand sqlSelect = new SqlCommand(sQuery, connection);
            bool connectionCloseOnExit = false;

            Clear();

            if (connection.State != System.Data.ConnectionState.Open)
            {
                connectionCloseOnExit = true;
                connection.Open();
            }

            SqlDataReader sqlReader = sqlSelect.ExecuteReader();

            _bEmpty = false;

            while (sqlReader.Read())
            {
                if (sqlReader["department"] == null || sqlReader["department"] == DBNull.Value)
                    continue;
                DepartmentRecord cr = new DepartmentRecord();
                cr.Name = sqlReader["department"].ToString().Trim();
                _Records.Add(cr);
            }
            sqlReader.Close();

            if (connectionCloseOnExit == true)
                connection.Close();
        }
    }
}
