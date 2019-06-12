using Notebook.Core.Data;
using Notebook.Core.DataSource;
using Notebook.Core.Export;
using Notebook.Core.Import;
using System.Collections.Generic;

namespace Notebook.Core.Model {
    public class NotebookModel {
        readonly AccountsExporter accountsExporter = new AccountsExporter();
        readonly RecordsExporter recordsExporter = new RecordsExporter();
        readonly AccountsImporter accountsImporter = new AccountsImporter();
        readonly RecordsImporter recordsImporter = new RecordsImporter();
        readonly AccountCollection accounts = new AccountCollection();
        readonly RecordCollection records = new RecordCollection();

        public void Import(string accountFullPathFile, string recordFullPathFile) {
            this.accounts.AddRange(this.accountsImporter.Import(accountFullPathFile));
            this.records.AddRange(this.recordsImporter.Import(recordFullPathFile));
        }
        public void Export(string accountFullPathFile, string recordFullPathFile) {
            this.accountsExporter.Export(accountFullPathFile, this.accounts);
            this.recordsExporter.Export(recordFullPathFile, this.records);
        }
        public void Import(NotebookDataSource source) {
            // TODO
        }
        public void Export(NotebookDataSource source) {
            // TODO
        }

        public IEnumerable<Record> Find(Account account) {
            // TODO
            return null;
        }
    }
}
