using MyWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        [HttpPost]
        public ActionResult load(MyModelData Data)
        {
            string connetionString = null;
            SqlConnection sqlCnn;
            SqlCommand sqlCmd;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            int i = 0;
            string sql = null;

            connetionString = Data.ConnectionString;
            sql = getquery(Data.Schema, Data.TableName);
            List<MyList> lst = new List<MyList>();
            sqlCnn = new SqlConnection(connetionString);
            try
            {
                sqlCnn.Open();
                sqlCmd = new SqlCommand(sql, sqlCnn);
                adapter.SelectCommand = sqlCmd;
                adapter.Fill(ds);
                var d1 = ds.Tables[0];
                var d2 = ds.Tables[1];
                var d3 = ds.Tables[2];
                var d4 = ds.Tables[3];
                var d5 = ds.Tables[4];
                var d6 = ds.Tables[5];
                var x = JsonConvert.SerializeObject(d1, Formatting.Indented);
                var y = JsonConvert.SerializeObject(d2, Formatting.Indented);
                var a = JsonConvert.SerializeObject(d3, Formatting.Indented);
                var b = JsonConvert.SerializeObject(d4, Formatting.Indented);
                var c = JsonConvert.SerializeObject(d5, Formatting.Indented);
                var d = JsonConvert.SerializeObject(d6, Formatting.Indented);
                MyList myList = new MyList();
                    myList.a = a;
                    myList.b = b;
                    myList.x = x;
                    myList.y = y;
                    myList.c = c;
                    myList.d = d;
                lst.Add(myList);
                adapter.Dispose();
                sqlCmd.Dispose();
                sqlCnn.Close();               
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return Json(msg);
            }           
            return Json(lst);       
        }
        private string getquery(string tableNamewithschema, string tableName)            
        {
            var query = "";
            query = " declare @tableNamewithschema nvarchar(200)='"+ tableNamewithschema + "',@tableName nvarchar(200)='"+ tableName + "',@query nvarchar(max)=null IF OBJECT_ID(N'tempdb..#tempTbl_Demo') IS NOT NULL DROP TABLE #tempTbl_Demo CREATE TABLE #tempTbl_Demo ( rowId INT IDENTITY(1,1), columnName NVARCHAR(50), datatType NVARCHAR(50), characterMaxLength nvarchar(50) null ) INSERT INTO #tempTbl_Demo SELECT COLUMN_NAME,DATA_TYPE,CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName and TABLE_SCHEMA=@tableNamewithschema ORDER BY ORDINAL_POSITION declare @ProcStart nvarchar(max)=null DECLARE @ProcName nvarchar(max)=null SELECT @ProcName = COALESCE(@ProcName , ' ')+',@'+columnName+' <span style='' text-transform: uppercase;''>'+datatType+' </span> '+'= NULL </br>' FROM #tempTbl_Demo set @ProcStart = CONCAT('<p> CREATE PROC [dbo].[sp_SPName] </br> ( </br> @flag NVARCHAR(100) </br>',@ProcName,' </br>) </br> AS </br> &nbsp;&nbsp; SET NOCOUNT ON; </br>&nbsp;&nbsp;SET XACT_ABORT ON; </br>&nbsp;&nbsp;BEGIN TRY </br> ' ) DECLARE @Insert nvarchar(max)=null DECLARE @InsertName nvarchar(max)=null SELECT @InsertName = COALESCE(@InsertName + ',</br>&nbsp;&nbsp;', '')+columnName FROM #tempTbl_Demo DECLARE @InsertValues nvarchar(max)=null SELECT @InsertValues = COALESCE(@InsertValues + ', </br>&nbsp;&nbsp;', '') + +'@'+columnName FROM #tempTbl_Demo set @Insert=('If (@Flag=''Insert'') </br>&nbsp;&nbsp; BEGIN </br>&nbsp;&nbsp; BEGIN TRAN </br>&nbsp;&nbsp; INSERT INTO </br>'+@tableNamewithschema+'.'+@tableName+' (</br>'+@InsertName+'</br>) </br></br>&nbsp;&nbsp; VALUES(</br>'+@InsertValues+'</br>) </br></br>&nbsp;&nbsp; IF(@@TRANCOUNT > 0 </br>&nbsp;&nbsp; AND @@ERROR = 0) </br>&nbsp;&nbsp; BEGIN </br>&nbsp;&nbsp; COMMIT TRAN; </br>&nbsp;&nbsp; SELECT ''0''AS ErrorCode,''Inserted Successfully'' as Msg </br>&nbsp;&nbsp; END;</br>&nbsp;&nbsp; END </br> ') declare @Update nvarchar(max)=null DECLARE @UpdateName nvarchar(max)=null SELECT @UpdateName = COALESCE(@UpdateName + ',</br>&nbsp;&nbsp; ', '')+columnName+'=@'+columnName FROM #tempTbl_Demo DECLARE @UpdateValues nvarchar(max)=null SELECT @UpdateValues = COALESCE(@UpdateValues + ',</br>&nbsp;&nbsp; ', '') + +'@'+columnName FROM #tempTbl_Demo set @Update=('If (@Flag=''Update'') </br></br>&nbsp;&nbsp; BEGIN </br></br>&nbsp;&nbsp; BEGIN TRAN </br>&nbsp;&nbsp; UPDATE '+@tableNamewithschema+'.'+@tableName+' SET '+@UpdateName+'</br>&nbsp;&nbsp; IF(@@TRANCOUNT > 0 </br>&nbsp;&nbsp; AND @@ERROR = 0) </br>&nbsp;&nbsp; BEGIN </br>&nbsp;&nbsp; COMMIT TRAN;</br>&nbsp;&nbsp; SELECT ''0'' AS ErrorCode,''Updated Successfully'' as Msg </br>&nbsp;&nbsp; END; </br>&nbsp;&nbsp; END </br> ') declare @List nvarchar(max)=null set @List=('If (@Flag=''List'') </br></br>&nbsp;&nbsp; BEGIN </br></br>&nbsp;&nbsp; </br>&nbsp;&nbsp;SELECT '+@InsertName+' FROM '+@tableNamewithschema+'.'+@tableName+' </br></br>&nbsp;&nbsp; END </br> ') declare @PopulateById nvarchar(max)=null set @PopulateById=('If (@Flag=''PopulateById'') </br></br>&nbsp;&nbsp; BEGIN </br></br>&nbsp;&nbsp; </br>&nbsp;&nbsp;SELECT '+@InsertName+' FROM '+@tableNamewithschema+'.'+@tableName+' </br></br>&nbsp;&nbsp; END </br> ') declare @Delete nvarchar(max)=null set @Delete=('If (@Flag=''Delete'') </br></br>&nbsp;&nbsp; BEGIN </br></br>&nbsp;&nbsp; BEGIN TRAN </br></br>&nbsp;&nbsp; UPDATE '+@tableNamewithschema+'.'+@tableName+' SET '+@UpdateName+' </br></br>&nbsp;&nbsp; IF(@@TRANCOUNT > 0 </br>&nbsp;&nbsp; AND @@ERROR = 0) </br>&nbsp;&nbsp; BEGIN </br>&nbsp;&nbsp; COMMIT TRAN; </br>&nbsp;&nbsp; SELECT ''0'' AS ErrorCode,''Deleted Successfully'' AS Msg </br>&nbsp;&nbsp; END; </br>&nbsp;&nbsp; END </br> ') set @query=concat(@Insert,' ',@Update,' ',@List,' ',@PopulateById,' ',@Delete) set @query=concat (@ProcStart,' ',@query,' END </br> TRY </br></br>&nbsp;&nbsp; BEGIN CATCH </br></br>&nbsp;&nbsp; IF @@TRANCOUNT > 0 </br>&nbsp;&nbsp; BEGIN </br>&nbsp;&nbsp; ROLLBACK TRANSACTION; </br>&nbsp;&nbsp; END; </br>&nbsp;&nbsp; DECLARE @msg VARCHAR(MAX); </br>&nbsp;&nbsp; SET @msg = ERROR_MESSAGE(); </br>&nbsp;&nbsp; SELECT ''1'' errorCode, </br>&nbsp;&nbsp; @msg AS errorMessage, </br>&nbsp;&nbsp; ERROR_LINE(); </br>&nbsp;&nbsp; END CATCH; </br>&nbsp;&nbsp;</p>') select @query SELECT CONCAT('common.',columnName,'= model.',columnName,';') FROM #tempTbl_Demo SELECT CONCAT('public string ',columnName,' {get;set;}') FROM #tempTbl_Demo SELECT CONCAT('sql += ',' ','\"@',columnName,'\" + objDao.FilterString(req.',columnName,'''); ') FROM #tempTbl_Demo SELECT CONCAT('lst.',columnName,' = item[\"',columnName,'\"].ToString();') FROM #tempTbl_Demo   SELECT concat('<div class=\"col-sm-4\"><div class=\"form-group\"><label class=\"control-label\"> @Html.LabelFor(m => m.',columnName,')<small class=\"text-danger\"> *</small></label> @Html.TextBoxFor(model => model.',columnName,', new { @class = \"form-control\", @required = \"\", @placeholder = \"\",autocomplete = \"off\"}) @Html.ValidationMessageFor(model => model.',columnName,', \"\", new { @class = \"error\" })</div></div>') FROM #tempTbl_Demo";
            return query;
        }
      }
}