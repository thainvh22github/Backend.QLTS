﻿using Microsoft.AspNetCore.Mvc;
using MISA.Web08.QLTS.API.Entities;
using MySqlConnector;
using Dapper;

namespace MISA.Web08.QLTS.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentController : Controller
    {

        /// <summary>
        /// API lấy danh sách toàn bộ phòng ban
        /// </summary>
        /// <returns>lấy danh sách toàn bộ phòng ban</returns>
        /// Author: NVHThai (16/09/2022)
        [HttpGet]
        [Route("")]
        public IActionResult GetAllDepartments()
        {
            try
            {
                // khởi tạo kết nối đến db
                string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nvhthai;Uid=root;Pwd=thaibqhg12;";
                var mySqlConnection = new MySqlConnection(connectionString);

                //khai báo tên stored procedure
                string storedProcedure = "Proc_department_GetAll";

                // Thực hiện gọi vào db
                var departments = mySqlConnection.Query(storedProcedure, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về từ DB

                return StatusCode(StatusCodes.Status200OK, departments);

                // Xử lý kết quả trả về từ DB
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "e001");
            }
        }
    }
}
