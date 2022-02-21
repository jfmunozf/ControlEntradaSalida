CREATE DEFINER=`root`@`localhost` PROCEDURE `CREAR_TABLA_INFORME_ES`()
BEGIN
	DECLARE vardocumento VARCHAR(255);
    DECLARE vardocumento_anterior VARCHAR(255);
    DECLARE varfecha DATE;
    DECLARE varfecha_anterior DATE;
    DECLARE varhora TIME;
    DECLARE varfinal INTEGER DEFAULT 0;   
    DECLARE updated BOOLEAN;
    DECLARE lastid INTEGER;
    DECLARE mydata CURSOR FOR SELECT fecha, hora, documento FROM entradas_salidas ORDER BY fecha, hora ASC;
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET varfinal = 1;
    DROP TABLE IF EXISTS temp_informe_es;
    CREATE TABLE temp_informe_es (id INTEGER NOT NULL PRIMARY KEY AUTO_INCREMENT, fecha DATE NOT NULL, hora_a TIME, hora_b TIME, documento VARCHAR(255) NOT NULL);
    
    OPEN mydata;
    SET vardocumento_anterior = "";
    SET varfecha_anterior ="1900-01-01";
    SET updated = 0;
    START TRANSACTION;
    iterar:LOOP
		FETCH mydata INTO varfecha, varhora, vardocumento;
        IF varfinal = 1 THEN
			LEAVE iterar;
		END IF;
        
        IF varfecha_anterior != varfecha THEN			
			INSERT INTO temp_informe_es (fecha, hora_a, documento) VALUES (varfecha, varhora, vardocumento);
			SET updated = 0;
		ELSE
			IF vardocumento_anterior = vardocumento AND updated = 1 THEN
				INSERT INTO temp_informe_es (fecha, hora_a, documento) VALUES (varfecha, varhora, vardocumento);
                SET updated = 0;
			ELSE				
				UPDATE temp_informe_es SET hora_b = varhora WHERE documento = vardocumento and fecha = varfecha and id = lastid;
                SET updated = 1;
			END IF;
		END IF;
        SET lastid = (SELECT MAX(id) FROM temp_informe_es);
        SET vardocumento_anterior = vardocumento;
		SET varfecha_anterior = varfecha;
	END LOOP iterar;
    COMMIT;
    CLOSE mydata;
END