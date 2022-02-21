CREATE DEFINER=`root`@`localhost` PROCEDURE `ELIMINAR_EMPLEADO`(IN doc VARCHAR(30))
BEGIN
	DELETE FROM entradas_salidas WHERE documento = doc;
    DELETE FROM empleados WHERE documento = doc;
END