using ayugAPItask.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ayugAPItask.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class ClientsController : ControllerBase
   {
      private readonly IConfiguration _configuration;

      public ClientsController(IConfiguration configuration)
      {
         _configuration = configuration;
      }

      [HttpGet]
      public JsonResult Get()
      {
         string query = @"
                        select Id,ClientName,CPF from
                        clients
         ";

         DataTable table = new DataTable();
         string sqlDataSource = _configuration.GetConnectionString("myConnection");
         MySqlDataReader myReader;
         using(MySqlConnection mycon=new MySqlConnection(sqlDataSource))
         {
            mycon.Open();
            using(MySqlCommand myCommand=new MySqlCommand(query, mycon))
            {
               myReader = myCommand.ExecuteReader();
               table.Load(myReader);

               myReader.Close();
               mycon.Close();
            }
         }

         return new JsonResult(table);
      }

      [HttpPost]
      public JsonResult Post(Client cliente)
      {
         string query = @"
                        insert into clients (ClientName,CPF) values
                                             (@Name, @Cpf);
                        
         ";

         DataTable table = new DataTable();
         string sqlDataSource = _configuration.GetConnectionString("myConnection");
         MySqlDataReader myReader;
         using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
         {
            mycon.Open();
            using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
            {
               myCommand.Parameters.AddWithValue("@Name", cliente.Name);
               myCommand.Parameters.AddWithValue("@Cpf", cliente.Cpf);
               myReader = myCommand.ExecuteReader();
               table.Load(myReader);

               myReader.Close();
               mycon.Close();
            }
         }

         return new JsonResult("Add Successfully!");
      }
   }
}
