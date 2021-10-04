
using DITS.HILI.WMS.MasterModel.Products;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;

namespace HILI.WEB.Commons.Helper
{
    public class GridHelper<T> where T : new()
    {
        private readonly StoreRequestParameters _prms;
        private readonly Dictionary<string, object> _filter = new Dictionary<string, object>();
        private readonly GridPanel _grid;
        private readonly string _strSearch = string.Empty;
        public GridHelper(GridPanel grdDataList, Dictionary<string, object> extraParams)
        {
            _prms = new StoreRequestParameters(extraParams);
            _filter = JSON.Deserialize<Dictionary<string, object>>(extraParams["filterheader"].ToString());
            _grid = grdDataList;
        }

        public int StartIndex => _prms.Start;
        public int Limit => _prms.Limit;
        public string Where => GetWhere();

        public T SearchModel => _Search();
        public List<DataFilterModel> FilterModel
        {
            get
            {
                List<DataFilterModel> result = new List<DataFilterModel>();
                int count = 1;
                foreach (KeyValuePair<string, object> item in _filter)
                {
                    DataFilterModel obj = JSON.Deserialize<DataFilterModel>(item.Value.ToString());
                    obj.DataIndex = item.Key;
                    obj.Condition = ConditionWhere.AND;
                    if (obj.Type == "date" || obj.Type == "number" || obj.Value == "true" || obj.Value == "false")
                    {
                        obj.Op = "=";
                    }
                    else
                    {
                        obj.Op = "LIKE";
                    }
                    obj.Group = count;
                    result.Add(obj);
                    count++;
                }
                return result;
                ;
            }
        }

        private T _Search()
        {
            ItemsCollection<ColumnBase> col = _grid.ColumnModel.Columns;

            Type SType = typeof(T);
            PropertyInfo[] SProperties = SType.GetProperties();
            T _searchModel = new T();

            foreach (ColumnBase item in col)
            {
                if (item.DataIndex == null)
                {
                    continue;
                }

                PropertyInfo _property = SProperties.First(x => x.Name == item.DataIndex);
                if (_property != null)
                {
                    try
                    {
                        _property.SetValue(_searchModel,
                                       Convert.ChangeType(_strSearch, _property.PropertyType),
                                       null);
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                }

            }

            return _searchModel;
        }
        public string Sort_Pagging => GetSortPagging();

        private string GetSortPagging()
        {
            if (_prms.Sort.Count() == 0)
            {
                return "";
            }

            string orderby = " ORDER BY ";
            foreach (DataSorter item in _prms.Sort)
            {
                if (orderby != " ORDER BY ")
                {
                    orderby += ",";
                }

                if (item.Direction == Ext.Net.SortDirection.ASC)
                {
                    orderby += " " + item.Property + " ASC ";
                }
                else
                {
                    orderby += " " + item.Property + " DESC ";
                }
            }

            if (_prms.Limit > 0)
            {
                orderby += "OFFSET  " + _prms.Start + " ROWS FETCH NEXT " + _prms.Limit + " ROWS ONLY ";
            }

            return orderby;
        }

        private string GetWhere()
        {
            return GetColumnSearch(_grid.ColumnModel.Columns, _strSearch);
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        private static string GetColumnSearch(ItemsCollection<ColumnBase> col, string strSearch)
        {
            if (col.Count == 0)
            {
                return "";
            }

            string colname = "";
            string strWhere = " WHERE ( ";
            foreach (ColumnBase item in col)
            {
                if (item.DataIndex != null)
                {

                    colname = ChangeColumnName(item);
                    if (colname == "")
                    {
                        continue;
                    }

                    if (strWhere != " WHERE ( ")
                    {
                        strWhere += " OR ";
                    }

                    strWhere += string.Format("{0} LIKE '%{1}%'", colname, strSearch);

                }
            }

            return strWhere + ") ";

        }

        private static string ChangeColumnName(ColumnBase item)
        {
            string oldvalue = item.DataIndex;
            //string langauge = AppsInfo.Langauge.ToLower() == "en" ? "en" : "th";
            switch (oldvalue)
            {

                case "Customer_Name":
                    oldvalue = "Customer_NameTH";
                    break;
                case "SupplierName":
                    oldvalue = "Supplier_NameTh";
                    break;
                case "Supplier_Name":
                    oldvalue = "Supplier_NameTh";
                    break;
                case "Location_TypeName":
                    oldvalue = "";
                    break;
                case "IsActive":
                    oldvalue = "";
                    break;
                case "Receive_Status_Receive_Name":
                    oldvalue = "ValueTH";
                    break;
                case "Dispatch_Status_Name":
                    oldvalue = "ValueTH";
                    break;
                case "SubCust_Name":
                    oldvalue = "SubCust_NameTh";
                    break;

                case "TotalItemInPallet":
                    oldvalue = "";
                    break;
                case "Receive_Urgent":
                    oldvalue = "";
                    break;

                case "TotalDispatchQty":
                    oldvalue = "";
                    break;
                case "TotalDispatchWeight":
                    oldvalue = "";
                    break;
                case "Job_Picking_Status_Name":
                    oldvalue = "";
                    break;
                case "IsUrgentString":
                    oldvalue = "";
                    break;

                //---------(Strat)-------------(Use in Contracts)------------------
                case "IsCustomer":
                    oldvalue = "";
                    break;
                case "IsSupplier":
                    oldvalue = "";
                    break;
                //---------(End)-------------(Use in Contracts)------------------
                default:
                    break;
            }

            if (oldvalue == "")
            {
                return "";
            }

            if (!string.IsNullOrEmpty(item.TagHiddenName))
            {
                oldvalue = item.TagHiddenName + "." + oldvalue;
            }

            return oldvalue;

        }

        private Unit GetWidth(string width)
        {
            if (width == null || width == "")
            {
                return Unit.Pixel(100);
            }
            else
            {
                int _width = Convert.ToInt32(width);
                return Unit.Pixel(_width);
            }
        }

        private Alignment GetAlign(string align)
        {
            if (align == null || align == "")
            {
                return Alignment.Left;

            }
            else
            {
                switch (align.ToUpper())
                {
                    case "LEFT": return Alignment.Center;
                    case "CENTER": return Alignment.Center;
                    case "RIGHT": return Alignment.Right;
                    default: return Alignment.Left;
                }
            }
        }

    }


}