using GppApp.Models;
using GppApp.DbContext;
using System;
using System.Collections.Generic;
using OnlineRevision.Models;

namespace GppApp.Interface
{
    public interface ProjectsInterface : IDisposable
    {
        List<Projects> GetProjects();
        List<ProjectItems> GetAllProjectItems();
        List<ProjectsViewModel> GetProjectItems(string projectId);
        List<ProjectsViewModel> GetProjectItemsInPdf(string projectId);
        List<ProjectItems> GetItems(int id);
        List<ImgHistoryViewModel> GetImages(int projectId);
        List<ProjectHistory> ShowProjectsHistory(string projectId);
        List<ProjectItemHistory> ShowProjectItemsHistory(int id);
        void AddProjects(Projects aProject);
        void UpdateProjects(ProjectHistory aProjectHistory);
        void UpdateItems(ProjectItemHistory aProjectItemHistory);
        void AddImages(int id, string imgName);
        bool CheckImages(int id, string imgName);
        string AddProjectItems(ProjectItems aProjectItems);
        void DeleteProjectItems(string itemId);
        void AddConfirmedItems(string projectId, string itemId);
        void SubmitForApproval(string projectId, string itemId);
    }
}
