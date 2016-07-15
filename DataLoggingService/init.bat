@echo off
REM initialize the sqlite database with a table as entity framework can't
REM Author: Kevin Ross

sqlite3 bin\Debug\testing.sqlite <schema.sql
