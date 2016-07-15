#!/bin/sh

# Initialize the sqlite database as entity framework cannot
# Author: Kevin Ross

sqlite3 bin/Debug/testing.sqlite <schema.sql
