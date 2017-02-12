# ClipboardSqlFormatter
Converting dynamic sql to static sql on the fly

# How it works

This is a tray application that listens your **clipboard** changes. When you copy to clipboard dynamic sql string **beginning with 'sp_executesql'** then application try to convert is to static sql. Convert include two steps:

1. Replacing parameters by its values
1. Formatting sql structure to readable and easy to modify view

So when you copy such sql to clipboard from SQL Profiler to SQL Management studio (for example), then pasted text will be a static beautiful sql.

For example, copied string:

pasted string:

# Feautures

* If you need to copy sql without any modifications, right click on application tray icon and uncheck item 'Is enabled'
* Move mouse to application tray icon and you will see current state of application - is is enabled of disabled.
