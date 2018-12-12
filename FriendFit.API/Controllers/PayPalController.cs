using FriendFit.API.Models;
using FriendFit.API.PayPal;
using FriendFit.Data.ApiModel.APIRequestModel;
using FriendFit.Data.IRepository;
using FriendFit.Data.Repository;
using PayPal;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FriendFit.API.Controllers
{
    [RoutePrefix("api/PayPal")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PayPalController : ApiController
    {
        private FriendFit.Data.FriendFitDBContext _objFriendFitDBEntity = new FriendFit.Data.FriendFitDBContext();
        public invoiceRepository Inovoice = new invoiceRepository();
        private _EmailController _ObjEmail = new _EmailController();

        [HttpPost]
        //Get payment paypal url
        [Route("GetPaymentPayPalURL_user")]
        public string GetPaymentPayPalURL_user(PayPalObjectInfo paypalObject)
        {
            string paypalURL = "";
            double cartAmount = 0;
            var itemList = new ItemList();
            var items = new List<Item>();
            //get the paypal api context            
            try
            {
                int AddMonths = Convert.ToInt32(paypalObject.up.DurationId);
                FriendFit.Data.UserInvitation _ui = new FriendFit.Data.UserInvitation();
                _ui.Userid = paypalObject.up.UserId;
                _ui.DeliveryTypeId = paypalObject.up.DeliveryMethodId;
                _ui.DurationId = paypalObject.up.DurationId;
                _ui.PurchaseDate = DateTime.Now;
                _ui.ExpiryDate = DateTime.Now.AddMonths(AddMonths);
                _ui.SubscriptionTypeId = paypalObject.up.SubscriptionTypeId;
                _ui.Cost = paypalObject.up.Cost;
                _ui.IsActive = false;
                _ui.IsRowActive = false;
                _ui.PaymentDone = 0;
                _objFriendFitDBEntity.UserInvitations.Add(_ui);
                _objFriendFitDBEntity.SaveChanges();
                string sku = Convert.ToString(_ui.Id);

                var apiContext = PayPalConfiguration.GetAPIContext();
                //map into paypal items list and get the total amount
                foreach (var cartItem in paypalObject.ProductList)
                {
                    if (cartItem.OrderQty > 0)
                    {
                        var Item = new Item();
                        Item.name = cartItem.Description;
                        Item.currency = paypalObject.Currency;
                        Item.price = Math.Round(cartItem.UnitPrice, 2).ToString();
                        Item.quantity = cartItem.OrderQty.ToString();
                        Item.sku = Convert.ToString(sku);
                        items.Add(Item);
                        cartAmount += Convert.ToDouble(Math.Round(cartItem.UnitPrice, 2)) * Convert.ToDouble(Item.quantity);
                    }
                }

                itemList.items = items;
                cartAmount = Math.Round(cartAmount, 2);

                var payer = new Payer() { payment_method = "paypal" };
                var redirUrls = new RedirectUrls()
                {
                    //cancel_url = paypalObject.SiteURL + "/api/PayPal/PaymentFail/?cancel=true",
                    //return_url = paypalObject.SiteURL + "/api/PayPal/GetPaymentDetails/"
                    cancel_url = "http://103.14.127.78:4900/#/dashboard/user-payment/?cancel=true",
                    return_url = "http://103.14.127.78:4900/#/dashboard/user-payment"

                    //cancel_url = "http://localhost:4200/#/dashboard/user-payment/?cancel=true",
                    //return_url = "http://localhost:4200/#/dashboard/user-payment"

                };

                var details = new Details()
                {
                    tax = paypalObject.Tax.ToString(),
                    shipping = paypalObject.ShippingFee.ToString(),
                    subtotal = cartAmount.ToString()
                };

                var paypalAmount = new Amount() { currency = paypalObject.Currency, total = cartAmount.ToString(), details = details };

                var transactionList = new List<Transaction>();
                Transaction transaction = new Transaction();
                transaction.description = paypalObject.OrderDescription;
                transaction.invoice_number = paypalObject.InvoiceNumber;
                transaction.amount = paypalAmount;
                transaction.item_list = itemList;
                transactionList.Add(transaction);

                var payment = new Payment()
                {
                    intent = "sale",
                    payer = payer,
                    transactions = transactionList,
                    redirect_urls = redirUrls
                };

                var createdPayment = payment.Create(apiContext);
                var links = createdPayment.links.GetEnumerator();
                while (links.MoveNext())
                {
                    var link = links.Current;
                    if (link.rel.ToLower().Trim().Equals("approval_url"))
                    {
                        paypalURL = link.href;
                    }
                }

            }
            catch (PaymentsException ex)
            {
                paypalURL = "ERROR: " + ex.Response;
            }

            return paypalURL;
        }

        [HttpPost]
        //Get payment paypal url
        [Route("GetPaymentPayPalURL")]
        public string GetPaymentPayPalURL(PayPalObjectInfo paypalObject)
        {
            string paypalURL = "";
            double cartAmount = 0;
            var itemList = new ItemList();
            var items = new List<Item>();
            //get the paypal api context
            try
            {
                var apiContext = PayPalConfiguration.GetAPIContext();
                //map into paypal items list and get the total amount
                foreach (var cartItem in paypalObject.ProductList)
                {
                    if (cartItem.OrderQty > 0)
                    {
                        var Item = new Item();
                        Item.name = cartItem.Description;
                        Item.currency = paypalObject.Currency;
                        Item.price = Math.Round(cartItem.UnitPrice, 2).ToString();
                        Item.quantity = cartItem.OrderQty.ToString();
                        Item.sku = Convert.ToString(cartItem.SKU);
                        items.Add(Item);
                        cartAmount += Convert.ToDouble(Math.Round(cartItem.UnitPrice, 2)) * Convert.ToDouble(Item.quantity);
                    }
                }
                itemList.items = items;
                cartAmount = Math.Round(cartAmount, 2);

                var payer = new Payer() { payment_method = "paypal" };
                var redirUrls = new RedirectUrls()
                {
                    //cancel_url = paypalObject.SiteURL + "/api/PayPal/PaymentFail/?cancel=true",
                    //return_url = paypalObject.SiteURL + "/api/PayPal/GetPaymentDetails/"
                    cancel_url = "http://103.14.127.78:4900/#/dashboard/payment/?cancel=true",
                    return_url = "http://103.14.127.78:4900/#/dashboard/payment"

                    //cancel_url = "http://localhost:4200/#/dashboard/payment/?cancel=true",
                    //return_url = "http://localhost:4200/#/dashboard/payment"


                };

                var details = new Details()
                {
                    tax = paypalObject.Tax.ToString(),
                    shipping = paypalObject.ShippingFee.ToString(),
                    subtotal = cartAmount.ToString()
                };

                var paypalAmount = new Amount() { currency = paypalObject.Currency, total = cartAmount.ToString(), details = details };

                var transactionList = new List<Transaction>();
                Transaction transaction = new Transaction();
                transaction.description = paypalObject.OrderDescription;
                transaction.invoice_number = paypalObject.InvoiceNumber;
                transaction.amount = paypalAmount;
                transaction.item_list = itemList;
                transactionList.Add(transaction);

                var payment = new Payment()
                {
                    intent = "sale",
                    payer = payer,
                    transactions = transactionList,
                    redirect_urls = redirUrls
                };

                var createdPayment = payment.Create(apiContext);
                var links = createdPayment.links.GetEnumerator();
                while (links.MoveNext())
                {
                    var link = links.Current;
                    if (link.rel.ToLower().Trim().Equals("approval_url"))
                    {
                        paypalURL = link.href;
                    }
                }

            }
            catch (PaymentsException ex)
            {
                paypalURL = "ERROR: " + ex.Response;
            }

            return paypalURL;
        }

        [HttpGet]
        //Get payment details
        [Route("GetPaymentDetails_User")]
        public string GetPaymentDetails_User(string paymentID)
        {
            var apiContext = PayPalConfiguration.GetAPIContext();
            var payment = new Payment();
            string str_payment = "";
            string FileSavedResult = "";
            string Result = "";

            try
            {
                payment = Payment.Get(apiContext, paymentID);
                FriendFit.Data.UserProductPayment tbl_payment = new FriendFit.Data.UserProductPayment();

                var payPalPaymentIdExist = _objFriendFitDBEntity.UserProductPayments.Where(s => s.PaypalId == payment.id).FirstOrDefault();

                if (payPalPaymentIdExist == null)
                {
                    if (payment.state == "created")
                    {
                        //  var GetPay
                        str_payment = "Payment success";

                        tbl_payment.PaypalId = payment.id;
                        tbl_payment.Intent = payment.intent;
                        tbl_payment.Cart = payment.cart;
                        tbl_payment.State = payment.state;
                        tbl_payment.Create_time = payment.create_time;
                        tbl_payment.Update_time = payment.update_time;
                        tbl_payment.Payer_PaymentMethod = payment.payer.payment_method;
                        tbl_payment.Status = payment.payer.status;
                        tbl_payment.Payer_info_Email = payment.payer.payer_info.email;
                        tbl_payment.Payer_info_FirstName = payment.payer.payer_info.first_name;
                        tbl_payment.Payer_info_LastName = payment.payer.payer_info.last_name;
                        tbl_payment.Payer_info_Payer_Id = payment.payer.payer_info.payer_id;
                        tbl_payment.Payer_info_Country_Code = payment.payer.payer_info.country_code;
                        //tbl_payment. = payment.payer.payer_info.country_code;

                        tbl_payment.Shipping_Address_Recipient_Name = payment.payer.payer_info.shipping_address.recipient_name;
                        tbl_payment.Shipping_Address_Line1 = payment.payer.payer_info.shipping_address.line1;
                        tbl_payment.Shipping_Address_Line2 = payment.payer.payer_info.shipping_address.line2;
                        tbl_payment.Shipping_Address_City = payment.payer.payer_info.shipping_address.city;
                        tbl_payment.Shipping_Address_Country_Code = payment.payer.payer_info.shipping_address.country_code;
                        tbl_payment.Shipping_Address_PostedCode = payment.payer.payer_info.shipping_address.postal_code;
                        tbl_payment.Shipping_Address_State = payment.payer.payer_info.shipping_address.state;

                        tbl_payment.Transactions_Description = payment.transactions[0].description;
                        tbl_payment.Transactions_InvoiceNum = payment.transactions[0].invoice_number;
                        tbl_payment.RowInsert = DateTime.UtcNow;
                        tbl_payment.UserId = Convert.ToInt32(payment.transactions[0].description);

                        _objFriendFitDBEntity.UserProductPayments.Add(tbl_payment);
                        _objFriendFitDBEntity.SaveChanges();

                        foreach (var item in payment.transactions)
                        {
                            int payid = _objFriendFitDBEntity.UserProductPayments.Where(x => x.PaypalId == payment.id).FirstOrDefault().Id;
                            foreach (var item_ in item.item_list.items)
                            {
                                FriendFit.Data.UserPurchaseProductsList pr = new FriendFit.Data.UserPurchaseProductsList();
                                pr.PaymentId = payid;
                                pr.SKU = item_.sku;
                                pr.Name = item_.name;
                                pr.Quantity = item_.quantity;
                                pr.Price = item_.price;
                                pr.Currency = item_.currency;
                                pr.RowInsert = DateTime.UtcNow;
                                _objFriendFitDBEntity.UserPurchaseProductsLists.Add(pr);
                                _objFriendFitDBEntity.SaveChanges();

                                Int64 newsku = Convert.ToInt64(item_.sku);
                                var IsPaymentDone = _objFriendFitDBEntity.UserInvitations.Where(x => x.Id == newsku).FirstOrDefault();
                                if (IsPaymentDone != null)
                                {
                                    IsPaymentDone.IsActive = true;
                                    IsPaymentDone.IsRowActive = true;
                                    IsPaymentDone.PaymentDone = 1;
                                    _objFriendFitDBEntity.Entry(IsPaymentDone).State = EntityState.Modified;
                                    _objFriendFitDBEntity.SaveChanges();

                                    var GetUserdetail = _objFriendFitDBEntity.UserProfiles.Where(x => x.Id == IsPaymentDone.Userid).FirstOrDefault();
                                    EmailModelAttachment tm = new EmailModelAttachment();
                                    tm.ToEmail = GetUserdetail.Email;
                                    if (GetUserdetail.LastName != null)
                                    {
                                        tm.CustomerName = GetUserdetail.FirstName + ' ' + GetUserdetail.LastName;
                                    }
                                    else
                                    {
                                        tm.CustomerName = GetUserdetail.FirstName;
                                    }
                                    tm.PayPalPaymentId = payment.id;
                                    tm.Subject = "Notifit Inovice";
                                    tm.FileURL = "/Invoice/" + payment.id + ".pdf";
                                    tm.IsSMS = _objFriendFitDBEntity.DeliveryTypeMasters.Where(x => x.Id == IsPaymentDone.DeliveryTypeId).FirstOrDefault().Name;
                                    tm.Isrecurringmonthly = _objFriendFitDBEntity.SubscriptionTypeMasters.Where(x => x.Id == IsPaymentDone.SubscriptionTypeId).FirstOrDefault().SubcriptionType;
                                    tm.ProductAmount = Convert.ToString(IsPaymentDone.Cost.Value.ToString("0.00"));

                                    if (GetUserdetail.CountryId == 61)
                                    {
                                        tm.includingGST = "Total price in USD including GST";
                                    }
                                    else
                                    {
                                        tm.includingGST = "Total price in USD ";
                                    }
                                    tm.TotalAmount = IsPaymentDone.Cost.Value.ToString("0.00");
                                    FileSavedResult = Inovoice.DownloadApplicationPDF(tm);
                                    Result = Inovoice.SendEmailWithAttachment(tm);
                                }
                            };
                        }
                    }
                    else
                    {
                        str_payment = "Payment Fail !";
                    }
                }
                else
                {
                    str_payment = "Payment Id Alrady Exists!";
                }

            }
            catch (PaymentsException ex)
            {
                //throw new Exception("Sorry there is an error getting the payment details. " + ex.Response);
                str_payment = "Sorry there is an error getting the payment details. " + ex.Response;
            }
            return str_payment + "------" + FileSavedResult + "-------------" + Result;
        }

        [HttpGet]
        //Get payment details
        [Route("GetPaymentDetails")]
        public string GetPaymentDetails(string paymentID)
        {
            var apiContext = PayPalConfiguration.GetAPIContext();
            var payment = new Payment();
            string str_payment = "";
            string Result = "";
            string FileSavedResult = "";
            try
            {
                payment = Payment.Get(apiContext, paymentID);
                FriendFit.Data.Payment tbl_payment = new FriendFit.Data.Payment();

                var payPalPaymentIdExist = _objFriendFitDBEntity.Payments.Where(s => s.PaypalId == payment.id).FirstOrDefault();

                if (payPalPaymentIdExist == null)
                {

                    if (payment.state == "created")
                    {
                        //  var GetPay
                        str_payment = "Payment success";

                        tbl_payment.PaypalId = payment.id;
                        tbl_payment.Intent = payment.intent;
                        tbl_payment.Cart = payment.cart;
                        tbl_payment.State = payment.state;
                        tbl_payment.Create_time = payment.create_time;
                        tbl_payment.Update_time = payment.update_time;
                        tbl_payment.Payer_PaymentMethod = payment.payer.payment_method;
                        tbl_payment.Status = payment.payer.status;
                        tbl_payment.Payer_info_Email = payment.payer.payer_info.email;
                        tbl_payment.Payer_info_FirstName = payment.payer.payer_info.first_name;
                        tbl_payment.Payer_info_LastName = payment.payer.payer_info.last_name;
                        tbl_payment.Payer_info_Payer_Id = payment.payer.payer_info.payer_id;
                        tbl_payment.Payer_info_Country_Code = payment.payer.payer_info.country_code;
                        //tbl_payment. = payment.payer.payer_info.country_code;

                        tbl_payment.Shipping_Address_Recipient_Name = payment.payer.payer_info.shipping_address.recipient_name;
                        tbl_payment.Shipping_Address_Line1 = payment.payer.payer_info.shipping_address.line1;
                        tbl_payment.Shipping_Address_Line2 = payment.payer.payer_info.shipping_address.line2;
                        tbl_payment.Shipping_Address_City = payment.payer.payer_info.shipping_address.city;
                        tbl_payment.Shipping_Address_Country_Code = payment.payer.payer_info.shipping_address.country_code;
                        tbl_payment.Shipping_Address_PostedCode = payment.payer.payer_info.shipping_address.postal_code;
                        tbl_payment.Shipping_Address_State = payment.payer.payer_info.shipping_address.state;

                        tbl_payment.Transactions_Description = payment.transactions[0].description;
                        tbl_payment.Transactions_InvoiceNum = payment.transactions[0].invoice_number;
                        tbl_payment.RowInsert = DateTime.UtcNow;
                        tbl_payment.UserId = Convert.ToInt32(payment.transactions[0].description);

                        _objFriendFitDBEntity.Payments.Add(tbl_payment);
                        _objFriendFitDBEntity.SaveChanges();

                        foreach (var item in payment.transactions)
                        {
                            int payid = _objFriendFitDBEntity.Payments.Where(x => x.PaypalId == payment.id).FirstOrDefault().Id;
                            Int64 newsku = 0;
                            foreach (var item_ in item.item_list.items)
                            {
                                FriendFit.Data.PurchaseProductsList pr = new FriendFit.Data.PurchaseProductsList();
                                pr.PaymentId = payid;
                                pr.SKU = item_.sku;
                                pr.Name = item_.name;
                                pr.Quantity = item_.quantity;
                                pr.Price = item_.price;
                                pr.Currency = item_.currency;
                                pr.RowInsert = DateTime.UtcNow;
                                _objFriendFitDBEntity.PurchaseProductsLists.Add(pr);
                                _objFriendFitDBEntity.SaveChanges();
                                newsku = Convert.ToInt64(item_.sku);

                                var IsPaymentDone = _objFriendFitDBEntity.FriendsInvitations.Where(x => x.Id == newsku).FirstOrDefault();
                                if (IsPaymentDone != null)
                                {
                                    IsPaymentDone.IsActive = true;
                                    IsPaymentDone.IsRowActive = true;
                                    IsPaymentDone.PaymentDone = 1;
                                    _objFriendFitDBEntity.Entry(IsPaymentDone).State = EntityState.Modified;
                                    _objFriendFitDBEntity.SaveChanges();
                                };
                            }
                        }

                        //-------------------------for Inovice
                        int PaymentRowId = _objFriendFitDBEntity.Payments.Where(x => x.PaypalId == payment.id).FirstOrDefault().Id;
                        var GetAllRows_Payment = _objFriendFitDBEntity.PurchaseProductsLists.Where(x => x.PaymentId == PaymentRowId).ToList();

                        string FriendsHTML = "";
                        EmailModelAttachment tm = new EmailModelAttachment();
                        var GetUserdetail = _objFriendFitDBEntity.UserProfiles.Where(a => a.Id == tbl_payment.UserId).FirstOrDefault();
                        decimal TotalAmt = 0;
                        foreach (var Payment_item in GetAllRows_Payment)
                        {

                            long id = Convert.ToInt64(Payment_item.SKU);
                            var GetuserId = _objFriendFitDBEntity.FriendsInvitations.Where(x => x.Id == id).FirstOrDefault();

                            tm.IsSMS = _objFriendFitDBEntity.DeliveryTypeMasters.Where(x => x.Id == GetuserId.DeliveryTypeId).FirstOrDefault().Name;
                            tm.Isrecurringmonthly = _objFriendFitDBEntity.SubscriptionTypeMasters.Where(x => x.Id == GetuserId.SubscriptionTypeId).FirstOrDefault().SubcriptionType;
                            tm.ProductAmount = Convert.ToString(GetuserId.Cost.Value.ToString("0.00"));
                            FriendsHTML += "<tr><td>Workout reminders via NotiFit app notifications to Friend " + GetuserId.FriendsName + "(" + tm.Isrecurringmonthly + " payment expiring " + GetuserId.ExpiryDate.Value.ToString("MM/dd/yyyy") + ")</td><td>$" + GetuserId.Cost.Value.ToString("0.00") + "</td></tr>";
                            TotalAmt += Convert.ToDecimal(GetuserId.Cost.Value.ToString("0.00"));
                        }

                        tm.ToEmail = GetUserdetail.Email;
                        tm.FriendsHTML = FriendsHTML;
                        if (GetUserdetail.LastName != null)
                            tm.CustomerName = GetUserdetail.FirstName + ' ' + GetUserdetail.LastName;
                        else
                            tm.CustomerName = GetUserdetail.FirstName;

                        tm.PayPalPaymentId = payment.id;
                        tm.Subject = "Notifit Friend Inovice";
                        tm.FileURL = "/FriendInvoice/" + payment.id + ".pdf";

                        if (GetUserdetail.CountryId == 61)
                        {
                            tm.includingGST = "Total price in USD including GST";
                        }
                        else
                        {
                            tm.includingGST = "Total price in USD ";
                        }

                        tm.TotalAmount = Convert.ToString(TotalAmt);
                        FileSavedResult = Inovoice.DownloadApplicationPDF_Friend(tm);
                         Result = Inovoice.SendEmailWithAttachment(tm);
                    }
                    else
                    {
                        str_payment = "Payment Fail !";
                    }
                }

                else
                {
                    str_payment = "Payment Id already exists";
                }
            }
            catch (PaymentsException ex)
            {
                //throw new Exception("Sorry there is an error getting the payment details. " + ex.Response);
                str_payment = "Sorry there is an error getting the payment details. " + ex.Response;
            }
            return str_payment + "------" + FileSavedResult + "-------------" + Result;
        }

        [HttpPost]
        //Get List of payment history
        [Route("GetPaymentHistory")]
        public PaymentHistory GetPaymentHistory(PayPalObjectInfo objPayPalobject)
        {
            var apiContext = PayPalConfiguration.GetAPIContext();
            var paymentHistory = new PaymentHistory();
            try
            {
                paymentHistory = Payment.List(apiContext, objPayPalobject.Count, objPayPalobject.StartID, objPayPalobject.StartIndex, objPayPalobject.StartTime, objPayPalobject.EndTime, objPayPalobject.StartDate, objPayPalobject.EndTime, objPayPalobject.PayeeEmail, objPayPalobject.PayeeID, objPayPalobject.SortBy, objPayPalobject.SortOrder);
            }
            catch (PaymentsException ex)
            {
                throw new Exception("Sorry there is an error getting the payment history. " + ex.Response);
            }
            return paymentHistory;
        }

        [HttpPost]
        //Process payment
        [Route("ProcessPayment")]
        public Payment ProcessPayment(PayPalObjectInfo objPayPalObject)
        {
            var apiContext = PayPalConfiguration.GetAPIContext();
            var paymentExecution = new PaymentExecution() { payer_id = objPayPalObject.PayerID };
            var payment = new Payment() { id = objPayPalObject.PaymentID };
            var executedPayment = new Payment();
            try
            {
                executedPayment = payment.Execute(apiContext, paymentExecution);
            }
            catch (PaymentsException ex)
            {
                throw new Exception("Sorry there is an error processing the payment. " + ex.Response);
            }

            return executedPayment;
        }


        [HttpGet]
        [Route("PaymentFail")]
        public string PaymentFail(bool cancel = true)
        {
            return "Payment Rejected by the user ";
        }


    }
}
