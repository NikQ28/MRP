DROP TABLE IF EXISTS "OrderStrings";
DROP TABLE IF EXISTS "Boms";
DROP TABLE IF EXISTS "Stocks";
DROP TABLE IF EXISTS "Orders";
DROP TABLE IF EXISTS "Items";

CREATE TABLE "Items"
(
	"Id" SERIAL PRIMARY KEY,
	"Name" VARCHAR(50) NOT NULL,
	"Description" VARCHAR(100) NULL
);

CREATE TABLE "Boms"
(
	"Id" SERIAL PRIMARY KEY,
	"ParentId" INT NOT NULL,
	"ComponentId" INT NOT NULL,
	"Count" INT NOT NULL DEFAULT 1 CHECK ("Count" > 0),
	FOREIGN KEY ("ParentId") REFERENCES "Items"("Id"),
	FOREIGN KEY ("ComponentId") REFERENCES "Items"("Id")
);

CREATE TABLE "Stocks"
(
	"Id" SERIAL PRIMARY KEY,
	"ItemId" INT NOT NULL,
	"Count" INT NOT NULL,
	"Operation" INTEGER NOT NULL CHECK ("Operation" IN (0, 1)),
	"Datetime" TIMESTAMP NOT NULL,
	FOREIGN KEY ("ItemId") REFERENCES "Items"("Id")
);

CREATE TABLE "Orders"
(
	"Id" SERIAL PRIMARY KEY,
	"Creation" TIMESTAMP NOT NULL,
	"Execution" TIMESTAMP NOT NULL,
	"Status" int NOT NULL CHECK ("Status" IN (0, 1)) 
);

CREATE TABLE "OrderStrings"
(
	"Id" SERIAL PRIMARY KEY,
	"OrderId" INT NOT NULL,
	"ItemId" INT NOT NULL,
	"Count" INT DEFAULT 1,
	FOREIGN KEY ("OrderId") REFERENCES 	"Orders"("Id"),
	FOREIGN KEY ("ItemId") REFERENCES "Items"("Id")
);

INSERT INTO "Items"("Name") VALUES
	('Электронные часы'),
	('Корпус'),
	('Электронная плата'),
	('Дисплей'),
	('Батарейка'),
	('Основание'),
	('Задняя крышка'),
	('Защитное стекло'),
	('Винт'),
	('Стеклянная пластина'),
	('Защитная плёнка'),
	('Печатная плата'),
	('Разъём'),
	('Транзистор'),
	('Микроконтроллер'),
	('Кварцевый генератор'),
	('LCD-панель'),
	('Модуль подсветки');


INSERT INTO "Boms"("ParentId", "ComponentId", "Count") VALUES 
	(1, 2, 1),
	(1, 3, 1),
	(1, 4, 1),
	(1, 5, 1),
	(2, 6, 1),
	(2, 7, 1),
	(2, 8, 1),
	(2, 9, 4),
	(8, 10, 1),
	(8, 11, 1),
	(3, 12, 1),
	(3, 15, 1),
	(3, 16, 1),
	(12, 13, 2),
	(12, 14, 50),
	(4, 17, 1),
	(4, 18, 2);
	