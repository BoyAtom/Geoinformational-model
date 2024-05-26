--
-- Файл сгенерирован с помощью SQLiteStudio v3.4.4 в Чт май 16 19:12:02 2024
--
-- Использованная кодировка текста: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Таблица: Company
CREATE TABLE Company (Key INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, Description TEXT);

-- Таблица: Enterprises
CREATE TABLE Enterprises (Key INTEGER PRIMARY KEY AUTOINCREMENT, Company INTEGER REFERENCES Company (Key), Industry INTEGER REFERENCES Industries (Key), Name TEXT NOT NULL, Description TEXT);

-- Таблица: Industries
CREATE TABLE Industries (Key INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL);

-- Таблица: Tags
CREATE TABLE Tags (Key INTEGER PRIMARY KEY AUTOINCREMENT, Enterprise INTEGER REFERENCES Enterprises (Key), X REAL NOT NULL, Y REAL NOT NULL);

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
