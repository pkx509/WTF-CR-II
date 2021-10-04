using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.MasterModel;
using DITS.HILI.WMS.ReceiveAPIs.Models;
using DITS.HILI.WMS.ReceiveModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DITS.HILI.WMS.ReceiveAPIs.Controllers
{
    public  partial  class ReceiveController : ApiController
    {
        [HttpGet]
        [Route("api/receiving/getMobileReceive")]
        public async Task<IHttpActionResult> getMobileReceive(ReceiveStatusEnum? receivestatusenum = null,string findValue="")
        {
            try
            {
                List<CustomEnumerable> status = service.GetReceiveStatus();

                int totalRow = 0;
                var _header = ApiHelpers.Response(Request);
                var receiveList = service.GetAll(_header.ProductOwnerID, receivestatusenum, null, null, null, out totalRow, null, null);

                #region search criteria
                if (!string.IsNullOrEmpty(findValue))
                {
                    receiveList = receiveList.Where(c => 
                                        c.ReceiveCode.Contains(findValue) || 
                                        c.PONumber.Contains(findValue) || 
                                        c.ContainerNo.Contains(findValue) || 
                                        c.Location.Code.Contains(findValue) || 
                                        c.Supplier.Name.Contains(findValue)).ToList();
                }

                #endregion


                List<mdlMobileReceive> mobileReceiveList = new List<mdlMobileReceive>();
                foreach (var receive in receiveList)
                {
                    #region make model list
                    var statusVal = status.Where(
                                        c => c.ID == Convert.ToInt32(receive.ReceiveStatus))
                                        .FirstOrDefault();
                    if (statusVal != null)
                    { 
                        mdlMobileReceive values = new mdlMobileReceive();
                        values.ReceiveID = receive.ReceiveID;
                        values.ReceiveCode = receive.ReceiveCode;
                        values.EstimateDate = receive.EstimateDate;
                        values.ContainerNo = receive.ContainerNo;
                        values.InvoiceNo = receive.InvoiceNo;
                        values.Location_Code = receive.Location.Code;
                        values.PONumber = receive.PONumber;
                        values.ReceiveStatus = receive.ReceiveStatus;
                        values.Supplier_Name = receive.Supplier.Name;
                        values.productCount = receive.TotalItems;
                        values.ReceiveStatus_Name = statusVal.Value; 
                        mobileReceiveList.Add(values);
                    }

                    #endregion
                }


                var responseMsg = Request.CreateResponse(HttpStatusCode.OK, mobileReceiveList);
                responseMsg.Headers.Add("X-TotalRecords", totalRow.ToString());
                IHttpActionResult response = ResponseMessage(responseMsg);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("api/receiving/getMobileReceiveProducts")]
        public async Task<IHttpActionResult> getMobileReceiveProducts(string ReceiveCode)
        {
            try
            {
                List<mdlMobileReceiveProducts> mobileReceiveDetailList = new List<mdlMobileReceiveProducts>();

                int totalRow = 0;
                var _header = ApiHelpers.Response(Request);
                var receiveDetailList = service.GetByReceiveCode(ReceiveCode);

                foreach (var prodItem in receiveDetailList.ReceiveDetailCollection)
                {
                    #region
                    mdlMobileReceiveProducts value = new mdlMobileReceiveProducts();
                    value.ReceiveID = prodItem.ReceiveID;
                    value.ReceiveDetailID = prodItem.ReceiveDetailID;
                    value.receiveCode = receiveDetailList.ReceiveCode;
                    value.productCode = prodItem.ProductCode;
                    value.productName = prodItem.Product.Name;
                    value.quantity = prodItem.Quantity;
                    value.unitName = prodItem.ProductUOM.Name;
                    value.palletCode = prodItem.PalletCode;
                    mobileReceiveDetailList.Add(value);

                    #endregion
                }

                var responseMsg = Request.CreateResponse(HttpStatusCode.OK, mobileReceiveDetailList);
                responseMsg.Headers.Add("X-TotalRecords", totalRow.ToString());
                IHttpActionResult response = ResponseMessage(responseMsg);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("api/receiving/getMobileReceiveProductsDetail")]
        public async Task<IHttpActionResult> getMobileReceiveProductsDetail(string ReceiveCode,Guid ReceiveDetailID)
        {
            try
            {
                List<mdlMobileReceiveProductDetail> mobileReceiveProductDetailList = new List<mdlMobileReceiveProductDetail>();

                int totalRow = 0;
                var _header = ApiHelpers.Response(Request);
                var receiveDetailList = service.GetByReceiveCode(ReceiveCode);

                foreach (var prodItem in receiveDetailList.ReceiveDetailCollection.Where(c=>c.ReceiveDetailID== ReceiveDetailID))
                {
                    #region
                    mdlMobileReceiveProductDetail value = new mdlMobileReceiveProductDetail();
                    value.receiveID = prodItem.ReceiveID;
                    value.receiveDetailID = prodItem.ReceiveDetailID;
                    value.productID = prodItem.Product.ProductID;
                    value.productCode = prodItem.ProductCode;
                    value.productName = prodItem.Product.Name;
                    value.palletCode = prodItem.PalletCode;
                    value.lot = prodItem.Lot;
                    value.productRemark = prodItem.Remark;
                    value.productStatusID = prodItem.ProductStatusID;
                    value.productStatus_Code = prodItem.ProductStatus.Code;
                    value.productStatus_Name = prodItem.ProductStatus.Name;
                    value.productSubStatusID = prodItem.ProductSubStatusID;
                    value.productSubStatus_Code = prodItem.ProductSubStatus.Code;
                    value.productSubStatus_Name = prodItem.ProductSubStatus.Name;
                    value.quantity = prodItem.Quantity;
                    value.baseQuantity = prodItem.BaseQuantity;
                    value.conversionQty = prodItem.ConversionQty;
                    value.conversionQtyUnitCode = prodItem.ProductUOM.Code;
                    value.conversionQtyUnitName = prodItem.ProductUOM.Name;
                    value.quantityUnitID = prodItem.ProductUOM.ProductUnitID;
                    value.quantityUnitCode = prodItem.ProductUOM.Code;
                    value.quantityUnitName = prodItem.ProductUOM.Name;
                    value.locationID = receiveDetailList.Location.LocationID;
                    value.location_Code = receiveDetailList.Location.Code;
                    value.mfgDate = prodItem.ManufacturingDate;
                    value.expDate = prodItem.ExpirationDate;
                    mobileReceiveProductDetailList.Add(value);

                    #endregion
                }

                var responseMsg = Request.CreateResponse(HttpStatusCode.OK, mobileReceiveProductDetailList);
                responseMsg.Headers.Add("X-TotalRecords", totalRow.ToString());
                IHttpActionResult response = ResponseMessage(responseMsg);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
