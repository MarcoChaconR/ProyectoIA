const fs = require('fs');
const path = require('path');

for (const file of ['app.e2e.db', 'app.e2e.db-shm', 'app.e2e.db-wal']) {
  const database = path.join(__dirname, file);
  if (fs.existsSync(database)) {
    fs.unlinkSync(database);
  }
}
