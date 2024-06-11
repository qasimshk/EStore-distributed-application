namespace estore.api.Models.Aggregates.Employee;

using estore.api.Common;
using estore.api.Models.Aggregates.Employee.Entities;
using estore.api.Models.Aggregates.Employee.ValueObjects;
using estore.api.Models.Aggregates.Orders;

public sealed class Employee : AggregateRoot<EmployeeId>
{
    private readonly List<EmployeeTerritory> _employeeTerritories = [];

    private readonly List<Order> _orders = [];

    public string Title { get; }

    public string FirstName { get; }

    public string LastName { get; }

    public string TitleOfCourtesy { get; }

    public DateTime BirthDate { get; }

    public DateTime HireDate { get; }

    public Addresses EmployeeAddress { get; }

    public string HomePhone { get; }

    public string Extension { get; }

    public byte[] Photo { get; }

    public string Notes { get; }

    public int? ReportsTo { get; }

    public string PhotoPath { get; }

    public IReadOnlyList<Order>? Orders => _orders.AsReadOnly();

    public IReadOnlyList<EmployeeTerritory> EmployeeTerritories => _employeeTerritories.AsReadOnly();

    private Employee() { }

    public Employee(EmployeeId employeeId,
        string title,
        string firstName,
        string lastName,
        string titleOfCourtesy,
        DateTime birthDate,
        DateTime hireDate,
        string homePhone,
        string extension,
        byte[] photo,
        string notes,
        int? reportsTo,
        string photoPath,
        Addresses employeeAddress) : base(employeeId)
    {
        Title = title;
        FirstName = firstName;
        LastName = lastName;
        TitleOfCourtesy = titleOfCourtesy;
        BirthDate = birthDate;
        HireDate = hireDate;
        HomePhone = homePhone;
        Extension = extension;
        Photo = photo;
        Notes = notes;
        ReportsTo = reportsTo;
        PhotoPath = photoPath;
        EmployeeAddress = employeeAddress;
    }
}
