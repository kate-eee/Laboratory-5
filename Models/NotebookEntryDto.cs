using System.ComponentModel;

namespace Lab5.Models;

public class NotebookEntryDto : INotifyPropertyChanged
{
    private int _id;
    private string _name;
    private string _surname;
    private string _phoneNumber;
    private string _email;

    public int Id
    {
        get => _id;
        set
        {
            if (_id != value)
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
    }

    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    public string Surname
    {
        get => _surname;
        set
        {
            if (_surname != value)
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            if (_phoneNumber != value)
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            if (_email != value)
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public NotebookEntryDto()
    {
    }

    public NotebookEntryDto(NotebookEntry entry)
    {
        Id = entry.Id;
        Name = entry.Name;
        Surname = entry.Surname;
        PhoneNumber = entry.PhoneNumber;
        Email = entry.Email;
    }
}