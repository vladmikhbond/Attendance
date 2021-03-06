﻿# Назначение
Программа предназначена для проверки посещаемости занятий, проводимых на встречах Google meet.
Программой будут пользоваться преподаватели, проводящие занятия.
Информация о посещаемости накапливается в базе данных и в любой момент доступна преподавателю.

# Основной сценарий
Преподаватель создает поток студентов, в котором будет проверять посещаемость 
(поток - это любое подмножество зарегистрированных студентов).
Преподаватель может создать несколько потоков.

Во время встречи Google meet преподаватель сохраняет (как Webpage Complete) страницу встречи, на которой отображается список.

Сразу или позднее преподаватель завершает регистрацию посещаемости:
 - загружает в программу сохраненный html-файл страницы.
 - файл анализируется и определяется тот поток, для которого проходила проверка.
 - преподаватель видит предварительные результаты проверки и сохраняет результат в базе данных.
  
Если поток определен неправильно, нужно явно указать поток и увидеть новый результат.


# Пользователи
Некоторые преподаватели являются также и администраторами 

## Администраторы
Регистрируют преподавателей.
Поддерживают список студентов.

## Преподаватели
Регистрируются сами.
Создают потоки.
Проводят проверку на своих потоках.
Получают отчеты о своих потоках.

# БД

## Сущности

AspnetUsers - преподаватели

	Students
	  Id
	  Surname
	  Name
	  Nick
	  GroupId
 
	Groups
	  Id
	  Name

	Flows
	  Id
	  UserName
	  Name
 
	Checks
	  Id
	  When - дата, время
	  FlowId 
      
  
	FlowStudents 
   
	CheckStudents 

## Комментарии

Группы - никакой роли не играют, это просто необязательный атрибут студента.
Отношение между потоками и студентами - M : M.
Между препами и потоками - 1 : M.
Наличие строки в таблице CheckStudents означает присутствие студента. Строки уникальны.

# Данные

Список студентов вуза задается в виде TSV, один студент - одна строка.
(sample: Євген	Шаловило	Васильович	yevhen.shalovylo@nure.ua	ПЗПІ-20-9)
В строке могут быть заданы только два первых поля (Nick = f1 + ' ' + f2)
или только одно (Nick = f1).


  


