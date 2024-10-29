using System.ComponentModel.DataAnnotations;

namespace entities.models;

public class changeStatusModel
{
    public int purchaseOrderId { get; set; }
    public int currentStatusId { get; set; }
    public int newStatusId { get; set; }
    public string? comments { get; set; }
    public int userId { get; set; }
    public List<purchaseOrderItemsModel>? purchaseOrderItems { get; set; }
}