using BLL.Model;
using DAL;

namespace BLL;

public class ReportService
{
    private readonly ReportRepository _reportRepository;

    public ReportService(ReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public List<ReportItem> Generate(int orderId)
    {
        return _reportRepository.Generate(orderId);
    }
}