--
-- Файл сгенерирован с помощью SQLiteStudio v3.4.4 в Вт май 28 09:23:39 2024
--
-- Использованная кодировка текста: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Таблица: Colors
CREATE TABLE IF NOT EXISTS Colors (Enterprise INTEGER REFERENCES Enterprises (Key), Red INTEGER DEFAULT (0), Green INTEGER DEFAULT (0), Blue INTEGER DEFAULT (0));

-- Таблица: Company
CREATE TABLE IF NOT EXISTS Company (Enterprise INTEGER REFERENCES Enterprises (Key), Name TEXT NOT NULL, Description TEXT);

-- Таблица: Enterprises
CREATE TABLE IF NOT EXISTS Enterprises (Key INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, Description TEXT);

-- Таблица: Industries
CREATE TABLE IF NOT EXISTS Industries (Enterprise INTEGER REFERENCES Enterprises (Key), Name TEXT NOT NULL);

-- Таблица: Tags
CREATE TABLE IF NOT EXISTS Tags (Enterprise INTEGER REFERENCES Enterprises (Key), X REAL NOT NULL, Y REAL NOT NULL);

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
