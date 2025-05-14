using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Task2_3.Models;
using Task2_3.Views;

namespace Task2_3.ViewModels;

internal class NotesViewModel : IQueryAttributable
{
    public NotesViewModel()
    {
        AllNotes = new ObservableCollection<NoteViewModel>(Note.LoadAll().Select(n => new NoteViewModel(n)));
        NewCommand = new AsyncRelayCommand(NewNoteAsync);
        SelectNoteCommand = new AsyncRelayCommand<NoteViewModel>(SelectNoteAsync);
    }

    public ObservableCollection<NoteViewModel> AllNotes { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectNoteCommand { get; }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            var noteId = query["deleted"].ToString();
            var matchedNote = AllNotes.Where(n => n.Identifier == noteId).FirstOrDefault();

            // If note exists, delete it
            if (matchedNote != null)
                AllNotes.Remove(matchedNote);
        }
        else if (query.ContainsKey("saved"))
        {
            var noteId = query["saved"].ToString();
            var matchedNote = AllNotes.Where(n => n.Identifier == noteId).FirstOrDefault();

            // If note is found, update it
            if (matchedNote != null)
            {
                matchedNote.Reload();
                AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
            }
            // If note isn't found, it's new; add it.
            else
            {
                AllNotes.Insert(0, new NoteViewModel(Note.Load(noteId)));
            }
        }
    }

    private async Task NewNoteAsync()
    {
        await Shell.Current.GoToAsync(nameof(NotePage));
    }

    private async Task SelectNoteAsync(NoteViewModel note)
    {
        if (note != null)
            await Shell.Current.GoToAsync($"{nameof(NotePage)}?load={note.Identifier}");
    }
}