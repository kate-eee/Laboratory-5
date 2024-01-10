using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Lab5.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace Lab5.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private NotebookEntryDto _selectedContact;
    private ObservableCollection<NotebookEntryDto> _contacts;

    public ObservableCollection<NotebookEntryDto> Contacts
    {
        get => _contacts;
        set => this.RaiseAndSetIfChanged(ref _contacts, value);
    }

    public NotebookEntryDto SelectedContact
    {
        get => _selectedContact;
        set => this.RaiseAndSetIfChanged(ref _selectedContact, value);
    }

    public ReactiveCommand<Unit, Unit> AddContactCommand { get; }
    public ReactiveCommand<Unit, Unit> SaveChangesCommand { get; }

    public MainWindowViewModel()
    {
        Contacts = new ObservableCollection<NotebookEntryDto>();

        AddContactCommand = ReactiveCommand.Create(AddContact);
        SaveChangesCommand = ReactiveCommand.Create(SaveChanges);
        
        var dbContextOptions = new DbContextOptionsBuilder<NotebookContext>()
            .UseSqlite("Data Source=Notebook.db")
            .Options;
        
        using (var dbContext = new NotebookContext(dbContextOptions))
        {
            dbContext.Database.EnsureCreated();
        }
        LoadDataFromDatabase();
        
        SelectedContact = Contacts.FirstOrDefault();
    }

    private void LoadDataFromDatabase()
    {
        var dbContextOptions = new DbContextOptionsBuilder<NotebookContext>()
            .UseSqlite("Data Source=Notebook.db")
            .Options;
        
        using (var context = new NotebookContext(dbContextOptions))
        {
            var contactsFromDb = context.Notebook.ToList();
            
            foreach (var entry in contactsFromDb)
            {
                Contacts.Add(new NotebookEntryDto(entry));
            }
        }
    }

    private void AddContact()
    {
        var newContactDto = new NotebookEntryDto(new NotebookEntry { Id = -1 });
        Contacts.Add(newContactDto);
        SelectedContact = newContactDto;
    }

    private void SaveChanges()
    {
        var dbContextOptions = new DbContextOptionsBuilder<NotebookContext>()
            .UseSqlite("Data Source=Notebook.db")
            .Options;

        using (var context = new NotebookContext(dbContextOptions))
        {
            if (SelectedContact.Id == -1)
            {
                // create new
                var newContact = new NotebookEntry
                {
                    Name = SelectedContact.Name,
                    Surname = SelectedContact.Surname,
                    PhoneNumber = SelectedContact.PhoneNumber,
                    Email = SelectedContact.Email
                };

                context.Notebook.Add(newContact);
                context.SaveChanges();
                
                SelectedContact.Id = newContact.Id;
                
                var updatedContactDto = Contacts.FirstOrDefault(c => c.Id == -1);
                if (updatedContactDto != null)
                {
                    updatedContactDto.Id = newContact.Id;
                    updatedContactDto.Name = newContact.Name;
                    updatedContactDto.Surname = newContact.Surname;
                    updatedContactDto.PhoneNumber = newContact.PhoneNumber;
                    updatedContactDto.Email = newContact.Email;
                }
            }
            else
            {
                // update existing contact
                var existingContact = context.Notebook.Find(SelectedContact.Id);

                if (existingContact != null)
                {
                    existingContact.Name = SelectedContact.Name;
                    existingContact.Surname = SelectedContact.Surname;
                    existingContact.PhoneNumber = SelectedContact.PhoneNumber;
                    existingContact.Email = SelectedContact.Email;

                    context.SaveChanges();
                    
                    var updatedContactDto = Contacts.FirstOrDefault(c => c.Id == existingContact.Id);
                    if (updatedContactDto != null)
                    {
                        updatedContactDto.Name = existingContact.Name;
                        updatedContactDto.Surname = existingContact.Surname;
                        updatedContactDto.PhoneNumber = existingContact.PhoneNumber;
                        updatedContactDto.Email = existingContact.Email;
                    }
                }
            }
        }

        Console.WriteLine($"Saved changes for contact: {SelectedContact.Name} {SelectedContact.Surname}");
    }
}