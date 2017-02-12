# ClipboardSqlFormatter
Converting dynamic sql to static sql on the fly

# What is it?

This is a tray application that listens your **clipboard** changes. When you copy to clipboard dynamic sql **beginning with 'sp_executesql'** then application try to convert it to a static sql. Converting includes two steps:

1. Replacing parameters by its values
1. Formatting sql structure to readable and easy to modify view

So when you copy such sql from SQL Profiler to SQL Management studio (for example), pasted text will be a static beautiful sql. Cool)

For example, if copied string is:

```SQL
exec sp_executesql N' SELECT "obj"."CreateDateTime", "obj"."LastEditDateTime" FROM LDERC 
"doc" INNER JOIN LDObject "obj" ON ("doc"."ID" = "obj"."ID") LEFT OUTER JOIN LDJournal 
"ContainerID.jrn" ON ("doc"."JournalID" = "ContainerID.jrn"."ID") WHERE  ( "doc"."ID" 
= @V0 AND  ( "doc"."StateID" <> 5 AND "ContainerID.jrn"."Name" <> ''Hidden journal'' 
)  ) ',N'@V0 bigint',@V0=6815463'
```

pasted string is:

```SQL
SELECT "obj"."CreateDateTime"
	,"obj"."LastEditDateTime"
FROM LDERC "doc"
INNER JOIN LDObject "obj" ON ("doc"."ID" = "obj"."ID")
LEFT OUTER JOIN LDJournal "ContainerID.jrn" ON ("doc"."JournalID" = "ContainerID.jrn"."ID")
WHERE (
		"doc"."ID" = 6815463
		AND (
			"doc"."StateID" <> 5
			AND "ContainerID.jrn"."Name" <> 'Hidden journal'
			)
		)
```


# Features

* If you need to copy sql without any modifications, right click on application tray icon and uncheck item 'Is enabled'
* Move mouse to application tray icon and you will see current state of application - is is enabled of disabled.

# Dependencies

For formatting sql I use an awesome library [PoorMansTSqlFormatter](https://github.com/TaoK/PoorMansTSqlFormatter)
