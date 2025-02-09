using Minimarket.DTO.Dashboard;
using Minimarket.Entity;

namespace Minimarket.BLL.Dashboard.Interfaces;

public interface IDashboardBLL
{
    Result<DashboardDTO> Summary();
}