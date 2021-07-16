﻿using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;

namespace Web.Admin.DirectorManagement
{
    public partial class DeleteDirector : System.Web.UI.Page
    {
        private DirectorBLL directorBLL;
        protected DirectorInfo directorInfo;
        protected bool enableShowInfo;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            directorBLL = new DirectorBLL();
            enableShowInfo = false;
            enableShowResult = false;
            stateString = null;
            stateDetail = null;
            try
            {
                
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_DirectorList", null);

                if (CheckLoggedIn())
                {
                    if (!IsPostBack)
                    {
                        await GetDirectorInfo();
                        directorBLL.Dispose();
                    }
                }
                else
                {
                    Response.RedirectToRoute("Account_Login", null);
                    directorBLL.Dispose();
                }
            }
            catch (Exception ex)
            {
                directorBLL.Dispose();
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private bool CheckLoggedIn()
        {
            object obj = Session["userSession"];
            if (obj == null)
                return false;

            UserSession userSession = (UserSession)obj;
            return (userSession.role == "Admin" || userSession.role == "Editor");
        }

        private int GetDirectorId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return int.Parse(obj.ToString());
        }

        private async Task GetDirectorInfo()
        {
            int id = GetDirectorId();
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_DirectorList", null);
            }
            else
            {
                directorInfo = await directorBLL.GetDirectorAsync(id);
                if (directorInfo == null)
                    Response.RedirectToRoute("Admin_DirectorList", null);
                else
                    enableShowInfo = true;
            }
        }

        private async Task DeleteDirectorInfo()
        {
            int id = GetDirectorId();
            StateOfDeletion state = await directorBLL.DeleteDirectorAsync(id);
            if (state == StateOfDeletion.Success)
            {
                stateString = "Success";
                stateDetail = "Đã xóa đạo diễn thành công";
            }
            else if (state == StateOfDeletion.Failed)
            {
                stateString = "Failed";
                stateDetail = "Xóa đạo diễn thất bại";
            }
            else
            {
                stateString = "ConstraintExists";
                stateDetail = "Không thể xóa đạo diễn. Lý do: Đạo diễn này đang được sử dụng!";
            }
            enableShowResult = true;
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                await DeleteDirectorInfo();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            directorBLL.Dispose();
        }
    }
}