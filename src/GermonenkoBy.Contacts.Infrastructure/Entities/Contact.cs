using GermonenkoBy.Common.EntityFramework.Models;

namespace GermonenkoBy.Contacts.Infrastructure.Entities;

public class Contact : Core.Models.Contact, IChangeDateTrackingModel
{
}