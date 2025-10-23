using DevExpress.ExpressApp;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace THOR_V1.Module.DatabaseUpdate
{
    public class DBAccess
    {
        public int SampleItemGetMaxID(String custromerID, String sampleCollectionID)
        {
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@CustomerID", "@SampleCollectionID" }, new SqlDbType[] { SqlDbType.NVarChar, SqlDbType.NVarChar }, new object[] { custromerID, sampleCollectionID });
            object nRst = _db.ExecuteScalar("SampleItemGetMaxIDStp", CommandType.StoredProcedure, arrParam);
            return (int)nRst;
        }

        public int InsertSampleSummary(String strOid)
        {
            int Rst = 0;
            DBBase _db = new DBBase();

            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@ItemSampleID" }, new SqlDbType[] { SqlDbType.NVarChar }, new object[] { strOid });
            Rst = _db.ExecuteNonQuery("SampleSummaryInsertStp", CommandType.StoredProcedure, arrParam);

            return Rst;
        }

        public DataTable GetImportBOMByUserId(String strUserID)
        {
            DataSet ds;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.NVarChar }, new object[] { strUserID });
            ds = _db.ExecuteQuery("GetImportBOMByUserIdStp", CommandType.StoredProcedure, arrParam);

            return ds.Tables[0];
        }
        public void DeleteImportItemByUserId(String strUserID)
        {
            int ds = 0;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.NVarChar }, new object[] { strUserID });
            ds = _db.ExecuteNonQuery("DeleteImportItemByUserIdStp", CommandType.StoredProcedure, arrParam);
        }

        public String[] GetPOInfoFromSage(String itemCode)
        {
            String[] result = new String[3] { "", "", "" };
            DataSet ds;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@ItemCode" }, new SqlDbType[] { SqlDbType.NVarChar }, new object[] { itemCode });
            ds = _db.ExecuteQuery("GetPOInfoFromSageStp", CommandType.StoredProcedure, arrParam);
            if (ds != null)
            {
                DataTable tbl = ds.Tables[0];
                if (tbl.Rows.Count > 0)
                {
                    result[0] = tbl.Rows[0][0]?.ToString().Trim();
                    result[1] = tbl.Rows[0][1].ToString().Trim();
                    result[2] = tbl.Rows[0][2].ToString().Trim();
                }
            }


            return result;
        }


        public int DeleteImportBOMToItem(String strUserID)
        {
            int nRst = 0;
            DBBase _db = new DBBase();

            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.NVarChar }, new object[] { strUserID });
            nRst = _db.ExecuteNonQuery("DeleteImportBOMToItemStp", CommandType.StoredProcedure, arrParam);
            return nRst;
        }
        public int DeleteImportItem(String strUserID)
        {
            int nRst = 0;
            DBBase _db = new DBBase();

            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.NVarChar }, new object[] { strUserID });
            nRst = _db.ExecuteNonQuery("DeleteImportItemStp", CommandType.StoredProcedure, arrParam);
            return nRst;
        }

        public int ImportBOMToItem(String strItem, String strUserID)
        {
            int Rst = 0;
            DBBase _db = new DBBase();

            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@ItemID", "@UserID" }, new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar }, new object[] { strItem, strUserID });
            object oRst = _db.ExecuteScalar("ImportBOMToItemStp", CommandType.StoredProcedure, arrParam);

            if (oRst != null)
                Rst = (int)oRst;
            return Rst;
        }

        public string ImportItemBOM(String strUserID, bool modeAdd)
        {
            string Rst = "";
            DBBase _db = new DBBase();
            object oRst;

            if (modeAdd == false)
            {
                SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { strUserID });
                oRst = _db.ExecuteScalar("ImportItemBOMStp", CommandType.StoredProcedure, arrParam);
            }
            else
            {
                SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { strUserID });
                oRst = _db.ExecuteScalar("AddItemBOMStp", CommandType.StoredProcedure, arrParam);
            }


            if (oRst != null)
                Rst = oRst.ToString();
            return Rst;
        }

        public string ImportBOMPacking(String strUserID, string strItemCode_Version)
        {
            string Rst = "";
            DBBase _db = new DBBase();
            object oRst;


            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID", "@ParentCodeVersion" }, new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar }, new object[] { strUserID, strItemCode_Version });
            oRst = _db.ExecuteScalar("ImportBOMPackingStp", CommandType.StoredProcedure, arrParam);

            if (oRst != null)
                Rst = oRst.ToString();
            return Rst;
        }

        public string ImportComponentItem(String strUserID)
        {
            string nRst = "";
            DBBase _db = new DBBase();
            object oRst;


            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { strUserID });
            oRst = _db.ExecuteScalar("ImportComponentItemStp", CommandType.StoredProcedure, arrParam);

            if (oRst != null)
                nRst = oRst.ToString();
            return nRst;
        }

        //public void SendMailChangeBOM(string strItemCode, String oldComp = "", int oldQty = 0, String newComp = "", int newQty = 0)
        //{
        //    StringBuilder strHTML = new StringBuilder();
        //    strHTML.AppendLine(@"<div>Dear ALL,</div>");
        //    strHTML.AppendLine(@"<div>&nbsp;</div>");
        //    strHTML.AppendLine(@"<div> We have just changed the BOM of Item " + strItemCode + "!</div>");
        //    if (!String.IsNullOrEmpty(oldComp) || !String.IsNullOrEmpty(newComp))
        //    {
        //        strHTML.AppendLine(@"<div>&nbsp;</div>");
        //        strHTML.AppendLine(@"<div>Old Component : " + oldComp + " is changed to " + newComp + "</div>");
        //    }

        //    if (oldQty > 0 || newQty > 0)
        //    {
        //        strHTML.AppendLine(@"<div>&nbsp;</div>");
        //        strHTML.AppendLine(@"<div>Old Qty : " + oldQty + " is changed to new qty: " + oldQty + "</div>");
        //    }

        //    strHTML.AppendLine(@"<div>&nbsp;</div>");
        //    strHTML.AppendLine(@"<div>Please check in the Item Tracking History or ask the TECH Dept for getting details!</div>");
        //    strHTML.AppendLine(@"<div>&nbsp;</div>");
        //    strHTML.AppendLine(@"<div>Thanks and Best Regard,</div>");
        //    strHTML.AppendLine(@"<div>System Email</div>");

        //    String[] _mailaddress = GetEmailRequestByNum_External("all"); //System.Configuration.ConfigurationSettings.AppSettings.GetValues("manufacture_list");

        //    //MailMessage mail = new MailMessage("info@ardavn.com", "phuongnv@ardavietnam.com,phongpp@ardavietnam.com,lemuel@ardavietnam.com,rande@ardavietnam.com,sekar@ardavietnam.com,thao@encoreihf.com");
        //    string[] _subject;
        //    _subject = new string[] { "Alert Change of BOM" };


        //    //strPath = strPath + ".pdf";

        //    SendMailExc(strHTML.ToString(), _mailaddress, _subject);
        //}




        //public void SendMailChangeItemCode(string strOldItemCode, String strNewItemCode)
        //{
        //    StringBuilder strHTML = new StringBuilder();
        //    strHTML.AppendLine(@"<div>Dear ALL,</div>");
        //    strHTML.AppendLine(@"<div>&nbsp;</div>");
        //    strHTML.AppendLine($@"<div> We have just changed the ItemCode from {strOldItemCode} to {strNewItemCode} of Item !</div>");
        //    strHTML.AppendLine(@"<div>&nbsp;</div>");
        //    strHTML.AppendLine(@"<div>Please check BOM, FNS module!</div>");
        //    strHTML.AppendLine(@"<div>&nbsp;</div>");
        //    strHTML.AppendLine(@"<div>Thanks and Best Regard,</div>");
        //    strHTML.AppendLine(@"<div>System Email</div>");

        //    String[] _mailaddress = GetEmailRequestByNum_External("all"); //System.Configuration.ConfigurationSettings.AppSettings.GetValues("manufacture_list");
        //    //MailMessage mail = new MailMessage("info@ardavn.com", "phuongnv@ardavietnam.com,phongpp@ardavietnam.com,lemuel@ardavietnam.com,rande@ardavietnam.com,sekar@ardavietnam.com,thao@encoreihf.com");
        //    string[] _subject;
        //    _subject = new string[] { "Alert Change ItemCode" };


        //    //strPath = strPath + ".pdf";

        //    SendMailExc(strHTML.ToString(), _mailaddress, _subject);
        //}

        //public int SendMailPURRequistion(string PurNum)
        //{
        //    String strDesc = GetPURInfoByNum(PurNum);

        //    if (!String.IsNullOrEmpty(strDesc))
        //    {
        //        StringBuilder strHTML = new StringBuilder();
        //        strHTML.AppendLine(@"<div>Dear Manager,</div>");
        //        strHTML.AppendLine(@"<div>&nbsp;</div>");
        //        strHTML.AppendLine(@"<div>You have a PUR Requisition :" + PurNum + " pending approval!</div>");

        //        strHTML.AppendLine(@"<div>&nbsp;</div>");
        //        strHTML.AppendLine(@"<div>" + strDesc + "</div>");
        //        strHTML.AppendLine(@"<div>&nbsp;</div>");
        //        strHTML.AppendLine(@"<div>Thanks and Best Regard,</div>");
        //        strHTML.AppendLine(@"<div>System Email</div>");

        //        string[] _mailaddress = GetEmailAprovedByNum(PurNum);
        //        //MailMessage mail = new MailMessage("info@ardavn.com", "phuongnv@ardavietnam.com,phongpp@ardavietnam.com,lemuel@ardavietnam.com,rande@ardavietnam.com,sekar@ardavietnam.com,thao@encoreihf.com");
        //        string[] _subject;
        //        _subject = new string[] { "PUR Requisition For Approving" };


        //        //strPath = strPath + ".pdf";

        //        SendMailExc(strHTML.ToString(), _mailaddress, _subject);
        //    }

        //    return 1;
        //}
        //update by tuanpv 02/08/2025
        //ExecQuery.cs
        //public int SendMailPURRequistion(string PurNum, string approver, string createby, string Notes)
        //{
        //    // Lấy apiBaseUrl và email người duyệt (bắt buộc)
        //    string apiBaseUrl = ConfigurationManager.AppSettings["APIServer"].ToString();
        //    string[] emailApproverResult = GetEmailAprovedByNum(PurNum);

        //    if (string.IsNullOrEmpty(apiBaseUrl) || emailApproverResult == null || emailApproverResult.Length == 0 || string.IsNullOrEmpty(emailApproverResult[0]))
        //    {
        //        Console.WriteLine("Lỗi: Thiếu ApiBaseUrl hoặc không tìm thấy email người duyệt.");
        //        return 0;
        //    }
        //    string approverEmail = emailApproverResult[0].Trim();

        //    // === BẮT ĐẦU PHIÊN BẢN NGẮN GỌN ===

        //    // Lấy chuỗi email creator/thu mua
        //    string[] rawEmailCreatorResult = GetEmailRequestByNum_External(createby);
        //    string creatorAndPurchaseEmailsString = (rawEmailCreatorResult != null && rawEmailCreatorResult.Length > 0) ? rawEmailCreatorResult[0] : "";

        //    // Tách chuỗi email thành danh sách
        //    List<string> emailList = creatorAndPurchaseEmailsString
        //        .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
        //        .Select(email => email.Trim())
        //        .ToList();

        //    // Lấy email thứ 2 (purEmail1), nếu không có thì dùng email mặc định
        //    string purEmail1 = emailList.ElementAtOrDefault(1) ?? "phantuan.tech@gmail.com";

        //    // Lấy email thứ 3 (purEmail2), nếu không có thì dùng email mặc định
        //    string purEmail2 = emailList.ElementAtOrDefault(2) ?? "phantuan.tech@gmail.com";

        //    // === KẾT THÚC PHIÊN BẢN NGẮN GỌN ===

        //    // Tạo URL approve và reject
        //    string approveUrl = $"{apiBaseUrl}/api/approve?PRNum={PurNum}&Creater={createby}&approver={approver}&approverEmail={approverEmail}&PurEmail1={purEmail1}&PurEmail2={purEmail2}";
        //    string rejectUrl = $"{apiBaseUrl}/api/reject?PRNum={PurNum}&Creater={createby}&approver={approver}&approverEmail={approverEmail}&PurEmail1={purEmail1}&PurEmail2={purEmail2}";

        //    // Tạo và gửi email (giữ nguyên logic)
        //    string htmlBody = BuildPurchaseRequisitionEmailHtml(PurNum, approveUrl, rejectUrl);
        //    if (string.IsNullOrEmpty(htmlBody)) return 0;

        //    SendMailExc(htmlBody, emailApproverResult, new string[] { $"PUR Requisition For Approving: {PurNum}" });

        //    return 1;
        //}

        //public async Task<string> GetActiveApiBaseUrlAsync()
        //{
        //    string configFilePath = Path.Combine(Application.StartupPath, "apiBaseUrl.txt");
        //    if (!File.Exists(configFilePath))
        //    {
        //        MessageBox.Show("Lỗi: Không tìm thấy file cấu hình apiBaseUrl.txt.", "Lỗi file", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return null;
        //    }

        //    // Đọc tất cả các URL từ file
        //    var urls = File.ReadAllLines(configFilePath)
        //                   .Where(line => !string.IsNullOrWhiteSpace(line))
        //                   .Select(line => line.Trim())
        //                   .ToList();

        //    if (!urls.Any())
        //    {
        //        MessageBox.Show("Lỗi: File apiBaseUrl.txt không chứa URL nào.", "Lỗi cấu hình", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return null;
        //    }

        //    // Sử dụng một HttpClient duy nhất để tăng hiệu năng
        //    using (var client = new HttpClient())
        //    {
        //        // Đặt timeout ngắn (ví dụ 5 giây) để không làm người dùng chờ lâu
        //        client.Timeout = TimeSpan.FromSeconds(5);

        //        // Lặp qua từng URL theo thứ tự ưu tiên
        //        foreach (var baseUrl in urls)
        //        {
        //            try
        //            {
        //                // Thử gửi một yêu cầu GET đơn giản đến một endpoint mà ta biết là tồn tại
        //                // (ví dụ /swagger/index.html). Chúng ta không cần đọc nội dung, chỉ cần biết nó có trả về OK hay không.
        //                var response = await client.GetAsync(baseUrl + "/swagger/index.html");

        //                // Nếu nhận được phản hồi (kể cả lỗi 404), có nghĩa là server đang chạy
        //                // IsSuccessStatusCode (200-299) là tốt nhất
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    Console.WriteLine($"API hoạt động tại: {baseUrl}");
        //                    return baseUrl; // Trả về URL đầu tiên hoạt động
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                // Bỏ qua lỗi (timeout, không kết nối được...) và thử URL tiếp theo
        //                Console.WriteLine($"Không thể kết nối đến {baseUrl}: {ex.Message}");
        //            }
        //        }
        //    }

        //    // Nếu không có URL nào hoạt động
        //    MessageBox.Show("Không thể kết nối đến bất kỳ máy chủ API nào được cấu hình.", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    return null;
        //}


        private string[] GetEmailRequestByNum_External(string createby_username)
        {
            string[] strResult = new string[] { "" };
            DBBase _db = new DBBase();
            // Sửa lại Stored Procedure và tham số
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserName" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { createby_username });
            DataSet ds = _db.ExecuteQuery("GetEmailByUsernameStp", CommandType.StoredProcedure, arrParam); // <<-- Dùng SP mới

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                strResult[0] = ds.Tables[0].Rows[0][0].ToString();
            }
            return strResult;
        }

        public string[] GetEmailByApprover_External(string createby_username)
        {
            string[] strResult = new string[] { "" };

            // Tạo một đối tượng DBBase mới. 
            // Lưu ý: Nếu lớp DBAccess của bạn đã có sẵn một đối tượng DBBase, hãy dùng nó.
            DBBase _db = new DBBase();

            // Chuẩn bị tham số để truyền vào Stored Procedure
            SqlParameter[] arrParam = _db.BSSqlParameter(
                new string[] { "@ApproverUserName" },
                new SqlDbType[] { SqlDbType.VarChar },
                new object[] { createby_username }
            );

            // Gọi Stored Procedure mới
            DataSet ds = _db.ExecuteQuery("GetEmailApproverByCreaterStp", CommandType.StoredProcedure, arrParam);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                // Lấy kết quả từ dòng đầu tiên, cột đầu tiên
                strResult[0] = ds.Tables[0].Rows[0][0].ToString();
            }

            return strResult;
        }

        public string[] GetEmailByApproverAndPur(string approverUserName)
        {
            string[] strResult = new string[] { "" };

            // Tạo một đối tượng DBBase mới. 
            // Lưu ý: Nếu lớp DBAccess của bạn đã có sẵn một đối tượng DBBase, hãy dùng nó.
            DBBase _db = new DBBase();

            // Chuẩn bị tham số để truyền vào Stored Procedure
            SqlParameter[] arrParam = _db.BSSqlParameter(
                new string[] { "@ApproverUserName" },
                new SqlDbType[] { SqlDbType.VarChar },
                new object[] { approverUserName }
            );

            // Gọi Stored Procedure mới
            DataSet ds = _db.ExecuteQuery("GetEmailByApproverAndPurStp", CommandType.StoredProcedure, arrParam);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                // Lấy kết quả từ dòng đầu tiên, cột đầu tiên
                strResult[0] = ds.Tables[0].Rows[0][0].ToString();
            }

            return strResult;
        }


        // Bạn có thể đặt hàm này trong lớp DBAccess hoặc một lớp tiện ích khác
        // Trong lớp DBAccess.cs hoặc lớp tiện ích của bạn

        // Trong lớp DBAccess.cs hoặc lớp tiện ích của bạn

        public string BuildPurchaseRequisitionEmailHtml(string PurNum, string approveUrl, string rejectUrl)
        {
            // === BƯỚC 1 & 2: GỌI SP VÀ ĐỌC DỮ LIỆU (Giữ nguyên) ===
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@PurNum" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { PurNum });
            DataSet ds = _db.ExecuteQuery("GetPurchaseRequisitionForEmailStp", CommandType.StoredProcedure, arrParam);

            if (ds == null || ds.Tables.Count < 2 || ds.Tables[0].Rows.Count == 0)
            {
                return "";
            }
            DataRow masterRow = ds.Tables[0].Rows[0];
            DataTable detailTable = ds.Tables[1];

            // === BƯỚC 3: XÂY DỰNG HTML VỚI KHOẢNG CÁCH GẦN HƠN ===
            var html = new StringBuilder();

            html.AppendLine(@"
        <center>
            <table width='100%' border='0' cellpadding='0' cellspacing='0' style='max-width: 700px; margin: 20px auto; font-family: Arial, sans-serif; color: #333; border: 1px solid #ddd;'>
                <!-- Header -->
                <tr>
                    <td style='background-color: #f8f8f8; padding: 20px; text-align: center; border-bottom: 1px solid #ddd;'>
                        <h1 style='margin: 0; font-size: 24px; color: #2a3a4b;'>Purchase Requisition Approval</h1>
                    </td>
                </tr>
                
                <!-- Main Content -->
                <tr>
                    <td style='padding: 30px;'>
                        <p style='margin: 0 0 20px 0; font-size: 16px;'>Dear Manager,</p>
                        <p style='margin: 0 0 20px 0; font-size: 16px;'>You have a new purchase requisition that requires your approval. Please review the details below.</p>
                        
                        <!-- Master Info Table (Padding đã được giảm) -->
                        <table width='100%' border='0' cellpadding='5' cellspacing='0' style='margin-bottom: 5px;'>
                            <tr>
                                <td style='padding: 2px; width: 150px; font-weight: bold; color: #555;'>Request No:</td>
                                <td style='padding: 2px;'>#" + masterRow["PRNum"] + @"</td>
                            </tr>
                            <tr>
                                <td style='padding: 2px; font-weight: bold; color: #555;'>Request Date:</td>
                                <td style='padding: 2px;'>" + ((DateTime)masterRow["RequestDate"]).ToString("dd/MM/yyyy") + @"</td>
                            </tr>
                            <tr>
                                <td style='padding: 2px; font-weight: bold; color: #555;'>Created By:</td>
                                <td style='padding: 2px;'>" + masterRow["CreatedBy"] + @"</td>
                            </tr>
                            <tr>
                                <td style='padding: 2px; font-weight: bold; color: #555;'>Request By:</td>
                                <td style='padding: 2px;'>" + masterRow["RequestBy"] + @"</td>
                            </tr>
                            <tr>
                                <td style='padding: 2px; font-weight: bold; color: #555;'>Department:</td>
                                <td style='padding: 2px;'>" + masterRow["RequestForDeptName"] + @"</td>
                            </tr>
                            <tr>
                                <td style='padding: 2px; font-weight: bold; color: #555; vertical-align: top;'>Notes:</td>
                                <td style='padding: 2px;'>" + masterRow["Notes"] + @"</td>
                            </tr>
                        </table>

                        <!-- Item Details Table (Padding đã được giảm) -->
                        <h2 style='font-size: 18px; color: #2a3a4b; border-bottom: 2px solid #eee; padding-bottom: 5px; margin-top: 10px;'>PR Details</h2>
                        <table width='100%' border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;'>
                            <thead>
                                <tr>
                                    <th style='border: 1px solid #ddd; padding: 8px; text-align: left; background-color: #f2f2f2;'>Description</th>
                                    <th style='border: 1px solid #ddd; padding: 8px; text-align: left; background-color: #f2f2f2;'>Purpose</th>
                                    <th style='border: 1px solid #ddd; padding: 8px; text-align: center; background-color: #f2f2f2;'>Qty</th>
                                </tr>
                            </thead>
                            <tbody>");

            // Lặp qua các dòng chi tiết
            foreach (DataRow detailRow in detailTable.Rows)
            {
                html.AppendLine($@"
                                <tr>
                                    <td style='border: 1px solid #ddd; padding: 8px;'>{detailRow["Description"]}</td>
                                    <td style='border: 1px solid #ddd; padding: 8px;'>{detailRow["PurposeofUse"]}</td>
                                    <td style='border: 1px solid #ddd; padding: 8px; text-align: center;'>{detailRow["Qty"]}</td>
                                </tr>");
            }

            html.AppendLine(@"
                            </tbody>
                        </table>

                                       <!-- Action Buttons -->
                    <table width='100%' border='0' cellpadding='0' cellspacing='0' style='margin-top: 40px; text-align: center;'>
                        <tr>
                            <td>
                                <!-- Approve Button -->
                                <a href='" + approveUrl + @"' target='_blank' style='display: inline-block; padding: 12px 30px; margin: 5px; font-size: 16px; color: #ffffff !important; background-color: #28a745; text-decoration: none; border-radius: 5px; font-weight: bold;'>Approve</a>
                                
                                <!-- Reject Button -->
                                <a href='" + rejectUrl + @"' target='_blank' style='display: inline-block; padding: 12px 30px; margin: 5px; font-size: 16px; color: #ffffff !important; background-color: #dc3545; text-decoration: none; border-radius: 5px; font-weight: bold;'>Reject</a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            
            <!-- Footer -->
            <tr>
                <td style='background-color: #f8f8f8; padding: 15px; text-align: center; font-size: 12px; color: #888; border-top: 1px solid #ddd;'>
                    This is an automated message from the system. Please do not reply directly to this email.
                </td>
            </tr>
        </table>
    </center>
");

            return html.ToString();
        }
        //end update by tuanpv
        //public int SendMailPURApproved(string PurNum)
        //{
        //    StringBuilder strHTML = new StringBuilder();
        //    strHTML.AppendLine(@"<div>Dear Manager,</div>");
        //    strHTML.AppendLine(@"<div>&nbsp;</div>");
        //    strHTML.AppendLine(@"<div>Your PUR Requisition :" + PurNum + " have been approved!!</div>");

        //    strHTML.AppendLine(@"<div>&nbsp;</div>");
        //    strHTML.AppendLine(@"<div>Thanks and Best Regard,</div>");
        //    strHTML.AppendLine(@"<div>System Email</div>");

        //    string[] _mailaddress = GetEmailRequestByNum(PurNum);
        //    //MailMessage mail = new MailMessage("info@ardavn.com", "phuongnv@ardavietnam.com,phongpp@ardavietnam.com,lemuel@ardavietnam.com,rande@ardavietnam.com,sekar@ardavietnam.com,thao@encoreihf.com");
        //    string[] _subject;
        //    _subject = new string[] { "PUR is Approved" };


        //    //strPath = strPath + ".pdf";

        //    SendMailExc(strHTML.ToString(), _mailaddress, _subject);
        //    return 1;
        //}

        private String GetPURInfoByNum(string PurNum)
        {
            String strPURReqInfo = "";
            //DataTable tbl = new DataTable();

            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@PurNum" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { PurNum });
            DataSet ds = _db.ExecuteQuery("GetPURInfoByNumStp", CommandType.StoredProcedure, arrParam);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                    strPURReqInfo = ds.Tables[0].Rows[0][0].ToString();
            }

            return strPURReqInfo;
        }

        public string ExecuteGetUserApprover(string createBy)
        {
            string result = string.Empty;
            DBBase _db = new DBBase();

            // Khai báo tham số đầu vào cho stored procedure
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@CreateBy" }, new SqlDbType[] { SqlDbType.NVarChar }, new object[] { createBy });

            // Thực thi stored procedure và lấy kết quả trả về (UserName)
            DataSet ds = _db.ExecuteQuery("GetUserApproverStp", CommandType.StoredProcedure, arrParam);

            // Kiểm tra và lấy dữ liệu trả về
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                result = ds.Tables[0].Rows[0]["UserName"].ToString();
            }

            return result;
        }

        private string[] GetEmailAprovedByNum(string PurNum)
        {
            string[] strResult = new string[] { "" };
            //DataTable tbl = new DataTable();

            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@PurNum" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { PurNum });
            DataSet ds = _db.ExecuteQuery("GetEmailAprovedByNumStp", CommandType.StoredProcedure, arrParam);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                    strResult[0] = ds.Tables[0].Rows[0][0].ToString();
            }

            return strResult;
        }

        private string[] GetEmailRequestByNum(string PurNum)
        {
            string[] strResult = new string[] { "" };
            //DataTable tbl = new DataTable();

            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@PurNum" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { PurNum });
            DataSet ds = _db.ExecuteQuery("GetEmailRequestByNumStp", CommandType.StoredProcedure, arrParam);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                    strResult[0] = ds.Tables[0].Rows[0][0].ToString();
            }

            return strResult;
        }

        public String GetMaxFinishinCode(string CustName)
        {
            String nRst = "";
            DBBase _db = new DBBase();

            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@CustName" }, new SqlDbType[] { SqlDbType.NVarChar }, new object[] { CustName });
            object oRst = _db.ExecuteScalar("GetMaxFinishinCodeStp", CommandType.StoredProcedure, arrParam);
            //MessageBox.Show(oRst.ToString());
            nRst = oRst.ToString();
            return nRst;
        }

        public int UpdateItemPackToItem(String strItemID)
        {
            int Rst = 0;
            DBBase _db = new DBBase();

            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@itemID" }, new SqlDbType[] { SqlDbType.NVarChar }, new object[] { strItemID });
            object oRst = _db.ExecuteScalar("UpdateItemPackToItemStp", CommandType.StoredProcedure, arrParam);

            if (oRst != null)
                Rst = (int)oRst;
            return Rst;


        }

        public int UpdateItemFinishCode(String strItemID, String strFinishListID)
        {
            int Rst = 0;
            DBBase _db = new DBBase();

            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@ItemID", "@FinishListID" }, new SqlDbType[] { SqlDbType.NVarChar, SqlDbType.NVarChar }, new object[] { strItemID, strFinishListID });
            object oRst = _db.ExecuteScalar("UpdateItemFinishCodeStp", CommandType.StoredProcedure, arrParam);

            if (oRst != null)
                Rst = (int)oRst;
            return Rst;

        }

        public int UpdateCategoryBOM(String strOldItemCode, String strNewItemCode)
        {
            int Rst = 0;
            DBBase _db = new DBBase();

            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@OldItemCode", "@NewItemCode" }, new SqlDbType[] { SqlDbType.NVarChar, SqlDbType.NVarChar }, new object[] { strOldItemCode, strNewItemCode });
            object oRst = _db.ExecuteScalar("UpdateCategoryBOMStp", CommandType.StoredProcedure, arrParam);

            if (oRst != null)
                Rst = (int)oRst;
            return Rst;

        }

        public int UnlockFns(String FinishID)
        {
            int Rst = 0;
            DBBase _db = new DBBase();

            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@FinishID" }, new SqlDbType[] { SqlDbType.NVarChar }, new object[] { FinishID });
            object oRst = _db.ExecuteScalar("UpdateUnlockFnsStp", CommandType.StoredProcedure, arrParam);

            if (oRst != null)
                Rst = (int)oRst;
            return Rst;

        }

        public int GetItemFromSage300()
        {
            int nRst = 0;
            DBBase _db = new DBBase();
            //SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@ComponentCode", "@Version", "@FilePath", "@IsCreateNew" }, new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.NVarChar, SqlDbType.Bit }, new object[] { strComponent, strBOMVersion, strPath, IsCreateNew });
            nRst = _db.ExecuteNonQuery("Sage300ThorItemsSysnStp", CommandType.StoredProcedure, null);
            return nRst;
        }

        public Dictionary<string, string> GetBomVersion(string _itemID)
        {
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@ItemID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { _itemID });

            SqlDataReader rd = _db.ExecuteReader("GetBOMVersionStp", CommandType.StoredProcedure, arrParam);
            Dictionary<string, string> cbo = new Dictionary<string, string>();
            if (rd != null)
            {
                while (rd.Read())
                {
                    cbo.Add(rd["VersionName"].ToString(), rd["VersionName"].ToString());
                }
            }
            rd.Close();
            rd.Dispose();
            _db.Close();
            return cbo;
        }

        public Dictionary<string, string> GetBomVersionAll()
        {
            DBBase _db = new DBBase();
            SqlDataReader rd = _db.ExecuteReader("GetBOMVersionAllStp", CommandType.StoredProcedure, null);
            Dictionary<string, string> cbo = new Dictionary<string, string>();
            if (rd != null)
            {
                while (rd.Read())
                {
                    cbo.Add(rd["Oid"].ToString(), rd["VersionName"].ToString());
                }
            }
            rd.Close();
            rd.Dispose();
            _db.Close();
            return cbo;
        }

        public Dictionary<string, string> GetItemFGAll()
        {
            DBBase _db = new DBBase();
            //SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@ItemCode" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { _itemCode });
            SqlDataReader rd = _db.ExecuteReader("GetItemFGAllStp", CommandType.StoredProcedure, null);
            Dictionary<string, string> cbo = new Dictionary<string, string>();
            if (rd != null)
            {
                while (rd.Read())
                {
                    cbo.Add(rd["Oid"].ToString(), rd["ItemCode"].ToString());
                }
            }
            rd.Close();
            rd.Dispose();
            _db.Close();
            return cbo;
        }

        public int CopyBOM(string fromItemCode, string fromVersion, string toItemCode, string toVersion)
        {
            int Rst = 0;
            DBBase _db = new DBBase();

            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@fromItemCode", "@fromVersion", "@toItemCode", "@toVersion" }, new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar }, new object[] { fromItemCode, fromVersion, toItemCode, toVersion });
            object oRst = _db.ExecuteScalar("CopyBOMStp", CommandType.StoredProcedure, arrParam);

            if (oRst != null)
                Rst = (int)oRst;
            return Rst;
        }

        public String GetThorVersion()
        {
            DBBase _db = new DBBase();
            //SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@ItemID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { _itemID });

            object rd = _db.ExecuteScalar("GetThorVersionStp", CommandType.StoredProcedure, null);

            if (rd == null)
            {
                rd = "1.0.9041.27372";
            }

            return rd.ToString();
        }

        public DataTable getItemPasteList(String psUserID)
        {
            DataTable tbl = new DataTable();

            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserId" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });
            DataSet ds = _db.ExecuteQuery("GetItemPasteListStp", CommandType.StoredProcedure, arrParam);
            if (ds != null)
                tbl = ds.Tables[0];

            return tbl;
        }

        public DataTable getReportNameItemPaste(String psUserID)
        {
            DataTable tbl = new DataTable();

            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserId" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });
            DataSet ds = _db.ExecuteQuery("GetReportNameItemPasteStp", CommandType.StoredProcedure, arrParam);
            if (ds != null)
                tbl = ds.Tables[0];

            return tbl;
        }

        public int UpdateItemPasteList(String strUserID)
        {
            int Rst = 0;
            DBBase _db = new DBBase();

            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@userID" }, new SqlDbType[] { SqlDbType.NVarChar }, new object[] { strUserID });
            object oRst = _db.ExecuteScalar("UpdateItemPasteListStp", CommandType.StoredProcedure, arrParam);

            if (oRst != null)
                Rst = (int)oRst;
            return Rst;

        }

        public int DeleteItemPaste(String strUserID)
        {
            int Rst = 0;
            DBBase _db = new DBBase();

            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@userID" }, new SqlDbType[] { SqlDbType.NVarChar }, new object[] { strUserID });
            object oRst = _db.ExecuteScalar("DeleteItemPasteStp", CommandType.StoredProcedure, arrParam);

            if (oRst != null)
                Rst = (int)oRst;
            return Rst;

        }

        public int ImportItemPriceHistory(string strUserID)
        {
            object nRst;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@userID" }, new SqlDbType[] { SqlDbType.NVarChar }, new object[] { strUserID });
            nRst = _db.ExecuteScalar("ImportItemPriceHistoryStp", CommandType.StoredProcedure, arrParam);
            if (nRst == null)
                nRst = -1;

            return (int)nRst;
        }

        public int CheckValidVersion(String _versionName)
        {
            int Rst = 0;
            DBBase _db = new DBBase();

            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@VersionName" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { _versionName });
            object oRst = _db.ExecuteScalar("CloneBOMStp", CommandType.StoredProcedure, arrParam);

            if (oRst != null)
                Rst = (int)oRst;
            return Rst;
        }

        public int SaveProformaToPO(string poid)
        {

            int nRst;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@ProformaPO", "@UserName" }, new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar }, new object[] { poid.ToString(), SecuritySystem.CurrentUserName.ToString() });
            nRst = _db.ExecuteNonQuery("ProformaToPOInsStp", CommandType.StoredProcedure, arrParam);
            return nRst;
        }

        public int UpdateSpecialRequest(string poid, string Request)
        {

            int nRst;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@CustPO", "@SpecialRequest" }, new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar }, new object[] { poid.ToString(), Request.ToString() });
            nRst = _db.ExecuteNonQuery("PODetailSpecialRequestUpdStp", CommandType.StoredProcedure, arrParam);
            return nRst;
        }

        public int UpdateProformaInvoice(string proformaID, string POTermID, string custCode)
        {
            int nRst;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@ProformaID", "@POTermID", "@CustCode" }, new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar }, new object[] { proformaID, POTermID, custCode });
            nRst = _db.ExecuteNonQuery("UpdateProformaInvoiceStp", CommandType.StoredProcedure, arrParam);
            return nRst;
        }

        //update by tuanpv 05/14/2025
        public int DeleteImportCommercialInvoice(String strUserID)
        {
            int Rst = 0;
            DBBase _db = new DBBase();

            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@userID" }, new SqlDbType[] { SqlDbType.NVarChar }, new object[] { strUserID });
            object oRst = _db.ExecuteScalar("DeleteImportCommercialInvoiceStp", CommandType.StoredProcedure, arrParam);

            if (oRst != null)
                Rst = (int)oRst;
            return Rst;

        }
        public DataTable getCommercialInvoicePasteList(String psUserID)
        {
            DataTable tbl = new DataTable();

            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });
            DataSet ds = _db.ExecuteQuery("getCommercialInvoicePasteListStp", CommandType.StoredProcedure, arrParam);
            if (ds != null)
                tbl = ds.Tables[0];

            return tbl;
        }
        public int ImportCommercialInvoice(String psUserID)
        {
            int nRst;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });
            nRst = _db.ExecuteNonQuery("ImportCommercialInvoiceStp", CommandType.StoredProcedure, arrParam);
            return nRst;
        }

        public int ImportSampleMonitoring(String psUserID)
        {
            int nRst;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });
            nRst = _db.ExecuteNonQuery("ImportSampleMonitoringStp", CommandType.StoredProcedure, arrParam);
            return nRst;
        }
        public int UpdateCommercialInvoicePasteList(String psUserID)
        {
            try
            {
                int nRst = 0;
                DBBase _db = new DBBase();
                SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });

                // Thay thế ExecuteScalar bằng ExecuteNonQuery
                nRst = _db.ExecuteNonQuery("UpdateCommercialInvoicePasteListStp", CommandType.StoredProcedure, arrParam);
                return nRst;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        //end update by tuanpv 05/14/2025


        //thêm vào dòng 945 trong class ExecQuery.cs 
        //-----------------------------//
        //-----------------------------//
        //-----------------------------//

        //update by tuanpv 09/13/2025

        public int DeleteItemPriceHistoryPasteList(String strUserID)
        {
            int Rst = 0;
            DBBase _db = new DBBase();

            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@userID" }, new SqlDbType[] { SqlDbType.NVarChar }, new object[] { strUserID });
            object oRst = _db.ExecuteScalar("DeleteItemPriceHistoryPasteListStp", CommandType.StoredProcedure, arrParam);

            if (oRst != null)
                Rst = (int)oRst;
            return Rst;

        }
        //end update by tuanpv 09/13/2025

        //-----------------------------//
        //-----------------------------//
        //-----------------------------//



        //thêm vào class ExecQuery.cs trong class DBAccess
        // update by tuanpv 27/05/2025

        public int DeleteImportPackingListInvoice(String strUserID)
        {
            int Rst = 0;
            DBBase _db = new DBBase();

            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@userID" }, new SqlDbType[] { SqlDbType.NVarChar }, new object[] { strUserID });
            object oRst = _db.ExecuteScalar("DeleteImportPackingListInvoiceStp", CommandType.StoredProcedure, arrParam);

            if (oRst != null)
                Rst = (int)oRst;
            return Rst;

        }
        public DataTable getPackingListInvoicePasteList(String psUserID)
        {
            DataTable tbl = new DataTable();

            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });
            DataSet ds = _db.ExecuteQuery("getPackingListInvoicePasteListStp", CommandType.StoredProcedure, arrParam);
            if (ds != null)
                tbl = ds.Tables[0];

            return tbl;
        }
        public int ImportPackingListInvoice(String psUserID)
        {
            int nRst;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });
            nRst = _db.ExecuteNonQuery("ImportPackingListStp", CommandType.StoredProcedure, arrParam);
            return nRst;
        }
        public int UpdatePackingListInvoicePasteList(String psUserID)
        {
            try
            {
                int nRst = 0;
                DBBase _db = new DBBase();
                SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });

                // Thay thế ExecuteScalar bằng ExecuteNonQuery
                nRst = _db.ExecuteNonQuery("UpdatePackingListInvoicePasteListStp", CommandType.StoredProcedure, arrParam);
                return nRst;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        //end update

        //update by tuanpv 30/05/2025

        // Thêm các method này vào class DBAccess

        public string GetNextCLNNumber()
        {
            DBBase _db = new DBBase();

            try
            {
                DataSet ds = _db.ExecuteQuery("GetNextCLNNumberStp", CommandType.StoredProcedure, null);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }

                return "1001"; // Default fallback
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting next CLN number: {ex.Message}", ex);
            }
            finally
            {
                _db?.Close();
            }
        }

        public int SaveChangeNoticeLabel(string clnNo, string changeType, string changeDetails,
            string previousVersion, DateTime date, string approvedBy, string userId)
        {
            DBBase _db = new DBBase();

            try
            {
                SqlParameter[] arrParam = _db.BSSqlParameter(
                    new string[] { "@CLNNO", "@ChangeType", "@ChangeDetails", "@PreviousVersion",
                          "@Date", "@ApprovedBy", "@UserID" },
                    new SqlDbType[] { SqlDbType.NVarChar, SqlDbType.NVarChar, SqlDbType.NVarChar,
                             SqlDbType.NVarChar, SqlDbType.DateTime,
                             SqlDbType.NVarChar, SqlDbType.NVarChar },
                    new object[] { clnNo ?? "", changeType ?? "", changeDetails ?? "",
                          previousVersion ?? "", date, approvedBy ?? "", userId ?? "" });

                DataSet ds = _db.ExecuteQuery("SaveChangeNoticeLabelStp", CommandType.StoredProcedure, arrParam);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(ds.Tables[0].Rows[0]["Result"]);
                }

                return -99; // No result returned
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving Change Notice Label: {ex.Message}", ex);
            }
            finally
            {
                _db?.Close();
            }
        }

        public int UpdateChangeNoticeLabel(string oid, string clnNo, string changeType, string changeDetails,
            string previousVersion, DateTime date, string approvedBy, string userId)
        {
            DBBase _db = new DBBase();

            try
            {
                // Validate Guid
                if (!Guid.TryParse(oid, out Guid guidOid))
                {
                    return -3; // Invalid Guid
                }

                SqlParameter[] arrParam = _db.BSSqlParameter(
                    new string[] { "@Oid", "@CLNNO", "@ChangeType", "@ChangeDetails", "@PreviousVersion",
                          "@Date",  "@ApprovedBy", "@UserID" },
                    new SqlDbType[] { SqlDbType.UniqueIdentifier, SqlDbType.NVarChar, SqlDbType.NVarChar,
                             SqlDbType.NVarChar, SqlDbType.NVarChar, SqlDbType.DateTime,
                             SqlDbType.NVarChar, SqlDbType.NVarChar },
                    new object[] { guidOid, clnNo ?? "", changeType ?? "", changeDetails ?? "",
                          previousVersion ?? "",  approvedBy ?? "", userId ?? "" });

                DataSet ds = _db.ExecuteQuery("UpdateChangeNoticeLabelStp", CommandType.StoredProcedure, arrParam);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(ds.Tables[0].Rows[0]["Result"]);
                }

                return -99; // No result returned
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating Change Notice Label: {ex.Message}", ex);
            }
            finally
            {
                _db?.Close();
            }
        }

        //end update 

        //public ItemInfos GetItemCodeNoImage()
        //{
        //    ItemInfos objs = new ItemInfos();
        //    ItemInfo obj = new ItemInfo();

        //    int i = 0;
        //    DataSet ds;
        //    DBBase _db = new DBBase();
        //    //SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@ItemCode" }, new SqlDbType[] { SqlDbType.NVarChar }, new object[] { itemCode });
        //    SqlDataReader rd = _db.ExecuteReader("GetItemCodeNoImageStp", CommandType.StoredProcedure, null);

        //    if (rd != null)
        //    {
        //        while (rd.Read())
        //        {
        //            while (rd.Read())
        //            {
        //                obj = new ItemInfo();
        //                obj.ItemCode = rd["ItemCode"].ToString();
        //                obj.CNTLACCT = rd["CNTLACCT"].ToString();

        //                objs.ItemArr.Add(obj);
        //            }
        //        }

        //    }
        //    rd.Close();
        //    rd.Dispose();
        //    _db.Close();
        //    return objs;
        //}

        public int UpdateItemImage()
        {
            String[] strPath = System.Configuration.ConfigurationSettings.AppSettings.GetValues("ExportImagePath");
            int Rst = 0;
            DBBase _db = new DBBase();

            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@path" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { strPath[0] });
            object oRst = _db.ExecuteScalar("UpdateItemImageStp", CommandType.StoredProcedure, arrParam);

            if (oRst != null)
                Rst = (int)oRst;
            return Rst;
        }

        public DataTable getSpecialPasteList(String psUserID)
        {
            DataTable tbl = new DataTable();

            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });
            DataSet ds = _db.ExecuteQuery("getSpecialPasteListStp", CommandType.StoredProcedure, arrParam);
            if (ds != null)
                tbl = ds.Tables[0];

            return tbl;
        }

        public DataTable getReportData(String psUserID)
        {
            DataTable tbl = new DataTable();
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });
            DataSet ds = _db.ExecuteQuery("ReportDataGetStp", CommandType.StoredProcedure, arrParam);
            if (ds != null)
                tbl = ds.Tables[0];

            return tbl;
        }

        public int DeleteSpecialPasteList(String psUserID)
        {
            int nRst;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });
            nRst = _db.ExecuteNonQuery("SpecialPasteListDelStp", CommandType.StoredProcedure, arrParam);
            return nRst;
        }

        public int ImportProformaInvoice(String psUserID)
        {
            int nRst;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });
            nRst = _db.ExecuteNonQuery("ImportProformaInvoiceStp", CommandType.StoredProcedure, arrParam);
            return nRst;
        }

        public int getPOFromExcel(string UserID)
        {
            int nRst;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { UserID });
            nRst = _db.ExecuteNonQuery("ImportPOFromExcelStp", CommandType.StoredProcedure, arrParam);
            return nRst;
        }

        public int GetInvoiceDetail(String InvoiceID, String CustomerID)
        {
            int nRst = 0;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@InvoiceID", "@CustomerID" }, new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar }, new object[] { InvoiceID, CustomerID });

            object obj = _db.ExecuteScalar("CreateInvoiceDetailByIDStp", CommandType.StoredProcedure, arrParam);

            if (obj != null)
                nRst = (int)obj;

            return nRst;
        }

        public int SynsBOMToSage(string UserID)
        {
            int nRst;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { UserID });
            nRst = _db.ExecuteNonQuery("SyncItemBOMToSageStp", CommandType.StoredProcedure, arrParam);
            return nRst;
        }

        public int CreateTagNewPO(String strUserID, String strNewPOID)
        {
            object nRst;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID", "@NewPOID" }, new SqlDbType[] { SqlDbType.NVarChar, SqlDbType.NVarChar }, new object[] { strUserID, strNewPOID });
            nRst = _db.ExecuteNonQuery("CreateTagNewPOStp", CommandType.StoredProcedure, arrParam);

            return (int)nRst;
        }

        public int DeleteTag(String strTagArr, string UserName)
        {
            DBBase _db = new DBBase();

            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@TagIDArr", "@UserName" }, new SqlDbType[] { SqlDbType.NVarChar, SqlDbType.VarChar }, new object[] { strTagArr, UserName });
            int oRst = _db.ExecuteNonQuery("DeleteTagItemPasteStp", CommandType.StoredProcedure, arrParam);

            return oRst;
        }

        //public void SendMailExc(String strHTML, string[] _mailaddress, string[] _subject, string strAttacthmentPath = "", string ccemail = "")
        //{
        //    //Name: phuongnv
        //    //Date: 03/05/2024
        //    //Description: Remove send email temporary
        //    try
        //    {
        //        using (MailMessage mail = new MailMessage())
        //        {
        //            mail.From = new MailAddress("talentohousevietnam@gmail.com");
        //            mail.To.Add(_mailaddress[0].ToString()); //_mailaddress[0]
        //            if (ccemail != "")
        //            {
        //                MailAddress cc = new MailAddress(ccemail);
        //                mail.CC.Add(cc);
        //            }
        //            mail.Subject = _subject[0];
        //            mail.Body = strHTML;

        //            mail.IsBodyHtml = true;

        //            if (!string.IsNullOrEmpty(strAttacthmentPath))
        //                mail.Attachments.Add(new Attachment(strAttacthmentPath));
        //            //mail.Attachments.Add(new Attachment("C:\\file.zip"));

        //            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
        //            {
        //                smtp.Credentials = new NetworkCredential("talentohousevietnam@gmail.com", "uovz syjw qgin zopu");
        //                smtp.EnableSsl = true;
        //                smtp.Send(mail);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Can not send email! \n" + ex.Message.ToString());
        //    }
        //}

        public DataTable getTagTransaction(String psUserID)
        {
            DataTable tbl = new DataTable();

            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });
            DataSet ds = _db.ExecuteQuery("getTagTransactionStp", CommandType.StoredProcedure, arrParam);
            if (ds != null)
                tbl = ds.Tables[0];

            return tbl;
        }

        public DataTable getTagCreating(String psUserID)
        {
            DataTable tbl = new DataTable();

            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });
            DataSet ds = _db.ExecuteQuery("getTagCreatingStp", CommandType.StoredProcedure, arrParam);
            if (ds != null)
                tbl = ds.Tables[0];

            return tbl;
        }

        public int DeleteTagTransaction(String psUserID)
        {
            int nRst;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });
            nRst = _db.ExecuteNonQuery("SwapItemTagDelStp", CommandType.StoredProcedure, arrParam);
            return nRst;
        }

        public int DeleteCreateTag(String psUserID)
        {
            int nRst;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });
            nRst = _db.ExecuteNonQuery("DelTagCreatingStp", CommandType.StoredProcedure, arrParam);
            return nRst;
        }

        public int DeleteInvoiceDetail(String InvoiceID)
        {
            int nRst;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@InvoiceID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { InvoiceID });
            nRst = _db.ExecuteNonQuery("DelInvoiceDetailStp", CommandType.StoredProcedure, arrParam);
            return nRst;
        }

        public int SwapItemTag(String psUserID)
        {
            object nRst;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });
            nRst = _db.ExecuteScalar("SwapItemTagStp", CommandType.StoredProcedure, arrParam);
            return (int)nRst;
        }

        public int TransactionTag(String psUserID)
        {
            object nRst;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });
            nRst = _db.ExecuteScalar("TransactionTagStp", CommandType.StoredProcedure, arrParam);

            if (nRst == null)
                nRst = -1;

            return (int)nRst;
        }

        public int CreateItemTag(String psUserID)
        {
            object nRst;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });
            nRst = _db.ExecuteScalar("CreateItemTagStp", CommandType.StoredProcedure, arrParam);

            if (nRst == null)
                nRst = -1;

            return (int)nRst;
        }

        public DataTable GetCreateShipment(String psUserID)
        {
            DataTable tbl = new DataTable();

            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });
            DataSet ds = _db.ExecuteQuery("GetCreateShipmentStp", CommandType.StoredProcedure, arrParam);
            if (ds != null)
                tbl = ds.Tables[0];

            return tbl;
        }

        public int DeleteCreateShipment(String psUserID)
        {
            int nRst;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });
            nRst = _db.ExecuteNonQuery("CreateShipmentDelStp", CommandType.StoredProcedure, arrParam);
            return nRst;
        }
        public int CreateShipment(String psUserID)
        {
            object nRst;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });
            nRst = _db.ExecuteScalar("CreateShipmentandTagFGToShipInsStp", CommandType.StoredProcedure, arrParam);
            if (nRst == null)
                nRst = -1;

            return (int)nRst;
        }

        //public void SendMailWhenShipment(string strShipmentID, string strUserName)
        //{
        //    string[] _mailaddress = System.Configuration.ConfigurationSettings.AppSettings.GetValues("mailshipment");
        //    //MailMessage mail = new MailMessage("info@ardavn.com", "phuongnv@ardavietnam.com,phongpp@ardavietnam.com,lemuel@ardavietnam.com,rande@ardavietnam.com,sekar@ardavietnam.com,thao@encoreihf.com");
        //    string[] _subject = new string[] { "Inform Shipment" };
        //    string[] _path = System.Configuration.ConfigurationSettings.AppSettings.GetValues("shipmentpath");

        //    string strHTML = @"<H2>ShipmentCode : " + strShipmentID + " đã được thực hiện vào lúc " + DateTime.Now.ToLongDateString() + " by " + strUserName + "! </H2>";
        //    strHTML = strHTML + "<br /> ";
        //    strHTML = strHTML + "<H2> " + _path[0] + DateTime.Now.Year.ToString() + @"\</H2>";

        //    SendMailExc(strHTML.ToString(), _mailaddress, _subject, "");
        //}

        //public UserInfos GetUserInfo(string strUserID)
        //{
        //    UserInfos objs = new UserInfos();
        //    UserInfo obj;
        //    DBBase _db = new DBBase();
        //    //SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@strToDay", "@strDateFrom", "@strDateTo", "@strOrderCode", "@strProductID", "@fTotal", "@strStatus", "@strPayMethod", "@strUserID", "@modID" }, new SqlDbType[] { SqlDbType.DateTime, SqlDbType.DateTime, SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Float, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar }, new object[] { dtToDay, dtDateFrom, dtDateTo, strOrderCode, strProductID, fTotal, strStatus, strPayMethod, strUserID, strFrom });
        //    SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@user_Id" }, new SqlDbType[] { SqlDbType.NVarChar }, new object[] { strUserID });

        //    SqlDataReader rd = _db.ExecuteReader("BarCodeGetUserInfoStp", CommandType.StoredProcedure, arrParam);
        //    if (rd != null)
        //    {
        //        while (rd.Read())
        //        {
        //            obj = new UserInfo();
        //            obj.UserName = rd["UserName"].ToString();
        //            obj.DepartmentName = rd["DepartmentName"].ToString();

        //            objs.UserInfoArr.Add(obj);
        //        }
        //    }
        //    rd.Close();
        //    rd.Dispose();
        //    _db.Close();
        //    return objs;
        //}

        public DataTable GetMasterPlanningUpdateStatus(String psUserID)
        {
            DataSet ds;
            DataTable myDataTable = new DataTable();
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });
            ds = _db.ExecuteQuery("GetMasterPlanningUpdateStatusStp", CommandType.StoredProcedure, arrParam);
            if (ds != null)
                myDataTable = ds.Tables[0];

            return myDataTable;
        }

        public int DelMasterPlanningUpdateStatus(String psUserID)
        {
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });
            int i = _db.ExecuteNonQuery("DelMasterPlanningUpdateStatusStp", CommandType.StoredProcedure, arrParam);

            return 0;
        }

        public int UpdateStatusMasterPlanning(String strUserID, string strMode)
        {
            object nRst;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID", "@Mode" }, new SqlDbType[] { SqlDbType.NVarChar, SqlDbType.VarChar }, new object[] { strUserID, strMode });
            nRst = _db.ExecuteNonQuery("MasterPlanningUpdateStatusStp", CommandType.StoredProcedure, arrParam);

            return (int)nRst;
        }

        //public TransactionTypes_DTO GetTransactionTypes(string strUserID)
        //{
        //    TransactionTypes_DTO objs = new TransactionTypes_DTO();
        //    TransactionType_DTO obj;

        //    DBBase _db = new DBBase();
        //    SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@user_Id" }, new SqlDbType[] { SqlDbType.NVarChar }, new object[] { strUserID });

        //    SqlDataReader rd = _db.ExecuteReader("BarCodeGetTransactionTypeStp", CommandType.StoredProcedure, arrParam);

        //    if (rd != null)
        //    {
        //        while (rd.Read())
        //        {
        //            obj = new TransactionType_DTO();
        //            obj.NumID = rd["NumID"].ToString();
        //            obj.TransactionCode = rd["TransactionCode"].ToString();

        //            objs.TransactionTypeArr.Add(obj);
        //        }
        //    }
        //    rd.Close();
        //    rd.Dispose();
        //    _db.Close();

        //    return objs;
        //}

        public int BarCodeTransactionExec(int nTagID, string strMode, string strUserId)
        {
            DBBase _db = new DBBase();

            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@tagID", "@mode", "@userID" }, new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.NVarChar }, new object[] { nTagID, strMode, strUserId });
            object nRst = _db.ExecuteScalar("BarCodeTransactionExecStp", CommandType.StoredProcedure, arrParam);

            return Convert.ToInt32(nRst);
        }

        public string GetMaxRequestNum()
        {
            object nRst;
            DBBase _db = new DBBase();
            //SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.VarChar }, new object[] { psUserID });
            nRst = _db.ExecuteScalar("GetMaxRequestNumStp", CommandType.StoredProcedure, null);
            return (string)nRst;
        }

        public string GetMaxItemCode(String ItemClassID, String BrandListID)
        {
            object nRst;
            string strRst = "";
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@BrandListID", "@ItemClassID" }, new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar }, new object[] { BrandListID, ItemClassID });
            nRst = _db.ExecuteScalar("GetNextItemCodeStp", CommandType.StoredProcedure, arrParam);
            if (nRst != null)
            {
                if (nRst.ToString() != "-1")
                    strRst = nRst.ToString();
            }

            return (string)strRst;
        }

        public int TagDayEnd()
        {
            DBBase _db = new DBBase();

            //SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@TagID" }, new SqlDbType[] { SqlDbType.Int }, new object[] { nTagID });
            int oRst = _db.ExecuteNonQuery("DayEndTodayStp", CommandType.StoredProcedure, null);

            return oRst;
        }

        public int CloneItem(string originalItemId, string newItemCode, String UserId, int itemMode)
        {
            DBBase _db = new DBBase();

            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID", "@ItemID", "NewItemCode", "@ItemMode" }, new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int }, new object[] { UserId, originalItemId, newItemCode, itemMode });
            object oRst = _db.ExecuteScalar("ItemCloneInsStp", CommandType.StoredProcedure, arrParam);

            return (int)oRst;
        }

        //public int ExcecuteQueryString(string strSQL)
        //{
        //    ReadConnectionString readName = new ReadConnectionString();
        //    String str = readName.ReadCnnStrName();
        //    SqlConnection connection = new SqlConnection();
        //    connection.ConnectionString = str;
        //    connection.Open();

        //    string myQuery = strSQL;
        //    SqlCommand myCommand = new SqlCommand();
        //    myCommand.CommandText = myQuery;
        //    myCommand.Connection = connection;
        //    myCommand.CommandType = CommandType.Text;
        //    //myCommand.Parameters.Add(strParameter, dataType).Value = strParameterValue;
        //    SqlParameter Param = myCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
        //    myCommand.CommandTimeout = 0;
        //    Param.Direction = ParameterDirection.ReturnValue;
        //    SqlDataReader rd = myCommand.ExecuteReader();
        //    int nResult = 0;
        //    if (rd != null)
        //    {
        //        while (rd.Read())
        //        {
        //            nResult = (int)rd[0];
        //        }
        //    }

        //    rd.Close();
        //    connection.Close();
        //    return nResult;
        //}

        public bool CheckApproveRequistion(String strRequestForDept, string createUser, string approveUser)
        {
            bool blnRst = false;
            object nRst;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@ReqForDept", "@createUser", "@approveUser" }, new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar }, new object[] { strRequestForDept, createUser, approveUser });
            nRst = _db.ExecuteScalar("CheckApproveRequistionStp", CommandType.StoredProcedure, arrParam);
            if (nRst == null)
                blnRst = false;
            else
            {

                blnRst = (int)nRst == 1 ? true : false;
            }
            return blnRst;
        }

        public int ImportPacking(String strUserID)
        {
            object nRst;
            DBBase _db = new DBBase();
            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.NVarChar, SqlDbType.VarChar }, new object[] { strUserID });
            nRst = _db.ExecuteNonQuery("ImportPackingStp", CommandType.StoredProcedure, arrParam);

            return (int)nRst;
        }

        public int DeleteImportPacking(String psUserID)
        {
            object nRst;
            DBBase _db = new DBBase();
            String myQuery = "delete from ImportPacking where UserID = '" + psUserID + "'";
            //SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.NVarChar, SqlDbType.VarChar }, new object[] { strUserID });
            nRst = _db.ExecuteNonQuery(myQuery, CommandType.Text, null);

            return (int)nRst;

        }

        //update by tuanpv 18/9/2025
        public int DeleteImportSampleMonitoring(String psUserID)
        {
            object nRst;
            DBBase _db = new DBBase();
            String myQuery = "delete from ImportSampleMonitoring where UserID = '" + psUserID + "'";
            //SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.NVarChar, SqlDbType.VarChar }, new object[] { strUserID });
            nRst = _db.ExecuteNonQuery(myQuery, CommandType.Text, null);

            return (int)nRst;

        }

        public int DeleteImportComponentItem(String psUserID)
        {
            object nRst;
            DBBase _db = new DBBase();
            String myQuery = "delete from ImportComponentItem where UserID = '" + psUserID + "'";
            //SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@UserID" }, new SqlDbType[] { SqlDbType.NVarChar, SqlDbType.VarChar }, new object[] { strUserID });
            nRst = _db.ExecuteNonQuery(myQuery, CommandType.Text, null);

            return (int)nRst;

        }

        public String GetMaxComponentCode(string strCompShortName)
        {
            String nRst = "";
            DBBase _db = new DBBase();

            SqlParameter[] arrParam = _db.BSSqlParameter(new string[] { "@ShortCode" }, new SqlDbType[] { SqlDbType.NVarChar }, new object[] { strCompShortName });
            object oRst = _db.ExecuteScalar("GetMaxComponentCodeStp", CommandType.StoredProcedure, arrParam);
            //MessageBox.Show(oRst.ToString());
            nRst = oRst.ToString();
            return nRst;
        }



    }

}
