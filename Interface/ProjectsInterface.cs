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
        List<ProjectsViewModel> GetProjectItems(string projectId);
        List<ProjectItems> GetItems(int id);
        List<ItemImages> GetImages(int projectId);
        List<ProjectHistory> ShowProjectsHistory(string projectId);
        List<ProjectItemHistory> ShowProjectItemsHistory(int id);
        void AddProjects(Projects aProject);
        void UpdateProjects(ProjectHistory aProjectHistory);
        void UpdateItems(ProjectItemHistory aProjectItemHistory);
        void AddImages(int id, string imgName);
        bool CheckImages(int id, string imgName);
        void AddProjectItems(ProjectItems aProjectItems);
    }
}
