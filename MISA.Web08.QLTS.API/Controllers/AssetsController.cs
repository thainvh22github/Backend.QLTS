// using: sử dụng các hàm
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web08.QLTS.API.Entities;
using MISA.Web08.QLTS.API.Entities.DTO;
using MISA.Web08.QLTS.API.Enums;
using MySqlConnector;
using MISA.Web08.QLTS.API.Attributes;
using Dapper;
using Microsoft.AspNetCore.Cors;
using MISA.Web08.QLTS.API.Properties;

namespace MISA.Web08.QLTS.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        #region Api get
        /// <summary>
        /// API lấy danh sách toàn bộ nhân viên
        /// </summary>
        /// <returns>Lấy danh sách toàn bộ tài sản</returns>
        /// Author: NVHThai (16/09/2022)
        [HttpGet]
        [Route("")]
        public IActionResult GetAllAssets()
        {

            try
            {
                //khởi tạo kết nối db
                string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nvhthai;Uid=root;Pwd=thaibqhg12;";
                var mySqlConnection = new MySqlConnection(connectionString);

                //khai báo tên stored procedure
                string storedProcedure = "Proc_asset_GetAll";


                // Thực hiện gọi vào db
                var assets = mySqlConnection.Query(storedProcedure, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về từ DB

                return StatusCode(StatusCodes.Status200OK, assets);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                    AssetErrorCode.Exception,
                    Resource.DevMsg_Exception,
                    Resource.DevMsg_Exception,
                    Resource.MoreInfo_Exception,
                    HttpContext.TraceIdentifier));
            }


        }


        /// <summary>
        /// Api lấy thông tin 1 tài sản theo id
        /// </summary>
        /// <param name="assetID">ID tài sản muốn lấy</param>
        /// <returns>Thông tin 1 tài sản</returns>
        /// Author: NVHThai (16/09/2022)
        [HttpGet]
        [Route("{assetID}")]
        public IActionResult GetEmployeeByID([FromRoute] Guid assetID)
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nvhthai;Uid=root;Pwd=thaibqhg12;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị tên Stored procedure
                string storedProcedureName = "Proc_asset_GetByAssetID";

                // Chuẩn bị tham số đầu vào cho stored procedure
                var parameters = new DynamicParameters();
                parameters.Add("@$v_AssetID", assetID);

                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                var asset = mySqlConnection.QueryFirstOrDefault<Assets>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về từ DB
                if (asset != null)
                {
                    return StatusCode(StatusCodes.Status200OK, asset);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                    AssetErrorCode.Exception,
                    Resource.DevMsg_Exception,
                    Resource.DevMsg_Exception,
                    Resource.MoreInfo_Exception,
                    HttpContext.TraceIdentifier));
            }
        }


        /// <summary>
        /// Hàm tìm kiếm và phân trang
        /// </summary>
        /// <param name="keword">tìm kiếm theo mã tài sản và tên tài sản</param>
        /// <param name="assetCategoryID">lọc theo id loại tài sản</param>
        /// <param name="departmentID">lọc theo id phòng ban</param>
        /// <param name="limit">số trang trong 1 bản ghi</param>
        /// <param name="offset">số trang</param>
        /// <returns>Danh sách tài sản</returns>
        [HttpGet("filter")]
        public IActionResult FilterAssets([FromQuery] string? keword, [FromQuery] Guid? assetCategoryID,
            [FromQuery] Guid? departmentID, [FromQuery] int limit = 20, [FromQuery] int offset = 1)
        {
            try
            {
                //khởi tạo kết nối db
                string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nvhthai;Uid=root;Pwd=thaibqhg12;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị tên Stored procedure
                string storedProcedureName = "Proc_asset_GetPaging";

                // Chuẩn bị tham số đầu vào cho stored procedure
                var parameters = new DynamicParameters();
                parameters.Add("@$v_Offset", (offset - 1) * limit);
                parameters.Add("@$v_Limit", limit);
                parameters.Add("@$v_Sort", "");


                var whereConditions = new List<string>();
                if (keword != null)
                {
                    whereConditions.Add($"fixed_asset_code LIKE '%{keword}%' OR fixed_asset_name LIKE '%{keword}%'");
                }
                if(departmentID != null)
                {
                    whereConditions.Add($"department_id LIKE '%{departmentID}%'");
                }
                if (assetCategoryID != null)
                {
                    whereConditions.Add($"fixed_asset_category_id LIKE '%{assetCategoryID}%'");
                }

                string whereClause = string.Join(" AND ", whereConditions);
                parameters.Add("@$v_Where", whereClause);

                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                var multipleResults = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về từ DB
                if (multipleResults != null)
                {
                    var asset = multipleResults.Read<Assets>();
                    var totalCount = multipleResults.Read<int>().Single();
                    return StatusCode(StatusCodes.Status200OK, new PaggingData<Assets>()
                    {
                        Data = asset.ToList(),
                        TotalCount = totalCount
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                    AssetErrorCode.Exception,
                    Resource.DevMsg_Exception,
                    Resource.DevMsg_Exception,
                    Resource.MoreInfo_Exception,
                    HttpContext.TraceIdentifier));
            }


        }


        /// <summary>
        /// API Lấy mã tài sản mới tự động tăng
        /// </summary>
        /// <returns>Mã nhân viên mới tự động tăng</returns>
        /// Author: NVHThai (16/09/2022)
        [HttpGet("new-code")]
        public IActionResult GetNewEmployeeCode()
        {

            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nvhthai;Uid=root;Pwd=thaibqhg12;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị tên stored procedure
                string storedProcedureName = "Proc_asset_GetMaxCode";

                // Thực hiện gọi vào DB để chạy stored procedure ở trên
                string maxAssetCode = mySqlConnection.QueryFirstOrDefault<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý sinh mã nhân viên mới tự động tăng
                // Cắt chuỗi mã nhân viên lớn nhất trong hệ thống để lấy phần số
                // Mã nhân viên mới = "NV" + Giá trị cắt chuỗi ở  trên + 1
                string newAssetCode = "TS-" + (Int64.Parse(maxAssetCode.Substring(3)) + 1).ToString();

                // Trả về dữ liệu cho client
                return StatusCode(StatusCodes.Status200OK, newAssetCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "e001");
            }
        }

        #endregion


        #region Api post
        /// <summary>
        /// Thêm mới 1 tài sản
        /// </summary>
        /// <param name="asset"></param>
        /// <returns>id của tài sản thêm mới</returns>
        /// Author: NVHThai (19/09/2022)
        [HttpPost]
        public IActionResult InsertAsset([FromBody] Assets asset)
        {

            try
            {
                // Validate dữ liệu đầu vào
                var properties = typeof(Assets).GetProperties();
                var validateFailures = new List<string>();
                foreach (var property in properties)
                {
                    string propertyName = property.Name;
                    var propertyValue = property.GetValue(asset);
                    var IsNotNullOrEmptyAttribute = (IsNotNullOrEmptyAttribute?)Attribute.GetCustomAttribute(property, typeof(IsNotNullOrEmptyAttribute));
                    if (IsNotNullOrEmptyAttribute != null && string.IsNullOrEmpty(propertyValue?.ToString()))
                    {
                        validateFailures.Add(IsNotNullOrEmptyAttribute.ErrorMessage);
                    }
                }

                if (validateFailures.Count > 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult(
                        AssetErrorCode.InvalidInput,
                        Resource.UserMsg_ValidateFailed,
                        Resource.UserMsg_ValidateFailed,
                        validateFailures,
                        HttpContext.TraceIdentifier));
                }




                //khởi tạo kết nối đến db mySQL
                string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nvhthai;Uid=root;Pwd=thaibqhg12;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // khai báo tên procdure insert
                var storedProcedureName = "Proc_asset_InsertOne";

                // chuẩn bị tham số đầu vào cho procedure
                var parameters = new DynamicParameters();
                
                var assetID = Guid.NewGuid();
                parameters.Add("$v_AssetID", assetID);
                parameters.Add("$v_AssetCode", asset.fixed_asset_code);
                parameters.Add("$v_AssetName", asset.fixed_asset_name);
                parameters.Add("$v_DepartmentID", asset.department_id);
                parameters.Add("$v_DepartmentCode", asset.department_code);
                parameters.Add("$v_DepartmentName", asset.department_name);
                parameters.Add("$v_AssetCategoryID", asset.fixed_asset_category_id);
                parameters.Add("$v_AssetCategoryCode", asset.fixed_asset_category_code);
                parameters.Add("$v_AssetCategoryName", asset.fixed_asset_category_name);
                parameters.Add("$v_PurchaseDate", asset.purchase_date);
                parameters.Add("$v_Cost", asset.cost);
                parameters.Add("$v_Quantity", asset.quantity);
                parameters.Add("$v_DepreciationRate", asset.depreciation_rate);
                parameters.Add("$v_TrackedYear", asset.tracked_year);
                parameters.Add("$v_LifeTime", asset.life_time);
                parameters.Add("$v_ProductionDay", asset.production_date);
                parameters.Add("$v_CreatedBy", "Nguyễn Vũ Hải Thái");
                parameters.Add("$v_CreateDate", DateTime.Now);
                parameters.Add("$v_ModifiedBy", "Nguyễn Vũ Hải Thái");
                parameters.Add("$v_ModifiedDate", DateTime.Now);

                //thực hiện gọi vào db để chạy procdure
                var numberOfAffectedRows = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // xử lý trả về dữ liệu
                if(numberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status201Created, assetID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                    AssetErrorCode.Exception,
                    Resource.DevMsg_Exception,
                    Resource.DevMsg_Exception,
                    Resource.MoreInfo_Exception,
                    HttpContext.TraceIdentifier));
            }
        }

        #endregion

        #region api put
        /// <summary>
        /// Sửa 1 tài sản
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="asset"></param>
        /// <returns>id của tài sản sửa</returns>
        /// Author: NVHThai (19/09/2022)
        [HttpPut("{assetID}")]
        public IActionResult UpdateAsset([FromRoute] Guid assetID, [FromBody] Assets asset)
        {
            try
            {
                // Validate dữ liệu đầu vào
                var properties = typeof(Assets).GetProperties();
                var validateFailures = new List<string>();
                foreach (var property in properties)
                {
                    string propertyName = property.Name;
                    var propertyValue = property.GetValue(asset);
                    var IsNotNullOrEmptyAttribute = (IsNotNullOrEmptyAttribute?)Attribute.GetCustomAttribute(property, typeof(IsNotNullOrEmptyAttribute));
                    if (IsNotNullOrEmptyAttribute != null && string.IsNullOrEmpty(propertyValue?.ToString()))
                    {
                        validateFailures.Add(IsNotNullOrEmptyAttribute.ErrorMessage);
                    }
                }

                if (validateFailures.Count > 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult(
                        AssetErrorCode.InvalidInput,
                        Resource.UserMsg_ValidateFailed,
                        Resource.UserMsg_ValidateFailed,
                        validateFailures,
                        HttpContext.TraceIdentifier));
                }


                //khởi tạo kết nối đến db mySQL
                string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nvhthai;Uid=root;Pwd=thaibqhg12;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // khai báo tên procdure insert
                var storedProcedureName = "Proc_asset_EditByID";

                // chuẩn bị tham số đầu vào cho procedure
                var parameters = new DynamicParameters();
                parameters.Add("@$v_AssetID", assetID);
                parameters.Add("$v_AssetCode", asset.fixed_asset_code);
                parameters.Add("$v_AssetName", asset.fixed_asset_name);
                parameters.Add("$v_DepartmentID", asset.department_id);
                parameters.Add("$v_DepartmentCode", asset.department_code);
                parameters.Add("$v_DepartmentName", asset.department_name);
                parameters.Add("$v_AssetCategoryID", asset.fixed_asset_category_id);
                parameters.Add("$v_AssetCategoryCode", asset.fixed_asset_category_code);
                parameters.Add("$v_AssetCategoryName", asset.fixed_asset_category_name);
                parameters.Add("$v_PurchaseDate", asset.purchase_date);
                parameters.Add("$v_Cost", asset.cost);
                parameters.Add("$v_Quantity", asset.quantity);
                parameters.Add("$v_DepreciationRate", asset.depreciation_rate);
                parameters.Add("$v_LifeTime", asset.life_time);
                parameters.Add("$v_ProductionDay", asset.production_date);
                parameters.Add("$v_ModifiedBy", "Nguyễn Vũ Hải Thái");
                parameters.Add("$v_ModifiedDate", DateTime.Now);

                //thực hiện gọi vào db để chạy procdure
                var numberOfAffectedRows = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // xử lý trả về dữ liệu
                if (numberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status201Created, assetID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                    AssetErrorCode.Exception,
                    Resource.DevMsg_Exception,
                    Resource.DevMsg_Exception,
                    Resource.MoreInfo_Exception,
                    HttpContext.TraceIdentifier));
            }
        }
        #endregion

        #region api delete

        /// <summary>
        /// Xóa 1 tài sản bằng id
        /// </summary>
        /// <param name="assetID"></param>
        /// <returns>id của tài sản xóa</returns>
        /// Author: NVHThai (19/09/2022)
        [HttpDelete("{assetID}")]
        public IActionResult DeleteAsset([FromRoute] Guid assetID)
        {
            try
            {
                //khởi tạo kết nối db
                string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nvhthai;Uid=root;Pwd=thaibqhg12;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị tên Stored procedure
                string storedProcedureName = "Proc_asset_DeleteByID";

                // Chuẩn bị tham số đầu vào cho stored procedure
                var parameters = new DynamicParameters();
                parameters.Add("@$v_AssetID", assetID);

                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                var numberOfAffectedRows = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về từ DB
                if (numberOfAffectedRows > 0)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, assetID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                    AssetErrorCode.Exception,
                    Resource.DevMsg_Exception,
                    Resource.DevMsg_Exception,
                    Resource.MoreInfo_Exception,
                    HttpContext.TraceIdentifier));
            }
        }

        /// <summary>
        /// Xóa nhiều tài sản
        /// </summary>
        /// <param name="assetIDs"></param>
        /// <returns>1 list các id tài sản vừa bị xóa</returns>
        [HttpPost("batch-delete")]
        public IActionResult DeleteMutipleAssets([FromBody] List<string> assetIDs)
        {
            return StatusCode(StatusCodes.Status200OK);
        }
        #endregion
    }
}
