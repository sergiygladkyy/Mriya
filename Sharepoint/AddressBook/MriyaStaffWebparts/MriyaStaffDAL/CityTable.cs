using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace MriyaStaffDAL
{
    [System.ComponentModel.DataObject(true)]
    public class CityTable
    {
        private List<CityRecord> _Records = new List<CityRecord>();
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

        public CityTable()
        {
        }

        public CityTable(SqlConnection connection)
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
        public List<CityRecord> GetRecords()
        {
            return _Records;
        }

        public void ReadFromDB(SqlConnection connection)
        {
            string sQuery = "SELECT DISTINCT f32_ad AS city " +
                "FROM UserFields407 " +
                "WHERE f32_ad IS NOT NULL AND f32_ad > '' " +
                "ORDER BY f32_ad";
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
                if (sqlReader["city"] == null || sqlReader["city"] == DBNull.Value)
                    continue;
                CityRecord cr = new CityRecord();
                cr.Name = sqlReader["city"].ToString().Trim();
                _Records.Add(cr);
            }
            sqlReader.Close();

            if (connectionCloseOnExit == true)
                connection.Close();
        }
    }
}
