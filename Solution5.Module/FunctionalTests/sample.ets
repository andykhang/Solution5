;#DropDB Solution5EasyTest

#Application Solution5.Win
#Application Solution5.Web

*FillForm
 User Name = Admin
*Action Log In

#IfDef Solution5.Win

*Action About...
 
*Action OK

#EndIf

#IfDef Solution5.Web
*Action Navigation(My Details)
#EndIf
