// globalSetup.js - Lenguaje: JavaScript (Playwright)
// Borra la base SQLite antes de cada corrida para que las pruebas E2E partan
// de un estado determinista (migración + seed frescos). La base se recrea
// automáticamente al arrancar la app (Program.cs ejecuta Database.Migrate()).
const fs = require('fs');
const path = require('path');

module.exports = async () => {
  const webDir = path.join(__dirname, '..', '..', 'src', 'ProyectoIA.Web');
  for (const file of ['app.db', 'app.db-shm', 'app.db-wal']) {
    const full = path.join(webDir, file);
    if (fs.existsSync(full)) {
      fs.unlinkSync(full);
    }
  }
};
