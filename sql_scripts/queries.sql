--скрипты для: выборки всех сотрудников, сотрудников у кого зп выше 10000, удаления
--сотрудников старше 70 лет, обновить зп до 15000 тем сотрудникам, у которых она меньше.

-- выборка всех сотрудников, сотрудников у кого зп выше 10_000
SELECT e."Id", e."Name", e."SalaryAdjustment" + p."BaseSalary" AS "salary"
FROM "Employees" AS e
    JOIN "Positions" AS p ON e."PositionId" = p."Id"
WHERE e."SalaryAdjustment" + p."BaseSalary" > 10_000;
---------------------------------------------------------------------------


--удаление сотрудников старше 70 лет
DELETE FROM "Employees" WHERE "Id" IN (SELECT "Id" FROM "Employees" WHERE (NOW() - "DateOfBirth") >= INTERVAL '70 years');
---------------------------------------------------------------------------


--обновление зп тем у кого она меньше 15_000
WITH underpaid_employees AS (
    SELECT e."Id",
           15_000 - (p."BaseSalary" + e."SalaryAdjustment") AS "delta"
    FROM "Employees" AS e
        JOIN "Positions" AS p ON e."PositionId" = p."Id"
    WHERE p."BaseSalary" + e."SalaryAdjustment" < 15_000
)
UPDATE "Employees" AS e
SET "SalaryAdjustment" = e."SalaryAdjustment" + underpaid_employees."delta"
FROM underpaid_employees
WHERE e."Id" = underpaid_employees."Id";
---------------------------------------------------------------------------