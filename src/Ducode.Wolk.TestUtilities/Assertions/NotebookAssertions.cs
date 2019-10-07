using Ducode.Wolk.Application.Notebooks.Models;
using Ducode.Wolk.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ducode.Wolk.TestUtilities.Assertions
{
    public static class NotebookAssertions
    {
        public static void ShouldBeEqual(Notebook notebook, NotebookDto notebookDto)
        {
            Assert.AreEqual(notebook.Id, notebookDto.Id);
            Assert.AreEqual(notebook.Name, notebookDto.Name);
            Assert.AreEqual(notebook.Created, notebookDto.Created);
        }
    }
}
