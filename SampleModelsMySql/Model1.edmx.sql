SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

CREATE SCHEMA IF NOT EXISTS `samplemodels` DEFAULT CHARACTER SET utf8 ;
USE `samplemodels` ;

-- -----------------------------------------------------
-- Table `samplemodels`.`department`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `samplemodels`.`department` (
  `idDepartment` INT(11) NOT NULL AUTO_INCREMENT,
  `Code` VARCHAR(45) NULL DEFAULT NULL,
  `Name` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`idDepartment`))
ENGINE = InnoDB
AUTO_INCREMENT = 6
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `samplemodels`.`employee`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `samplemodels`.`employee` (
  `idEmployee` INT(11) NOT NULL AUTO_INCREMENT,
  `Code` VARCHAR(45) NULL DEFAULT NULL,
  `Name` VARCHAR(45) NULL DEFAULT NULL,
  `MailAddress` VARCHAR(45) NULL DEFAULT NULL,
  `department_idDepartment` INT(11) NOT NULL,
  PRIMARY KEY (`idEmployee`, `department_idDepartment`),
  INDEX `fk_employee_department_idx` (`department_idDepartment` ASC),
  CONSTRAINT `fk_employee_department`
    FOREIGN KEY (`department_idDepartment`)
    REFERENCES `samplemodels`.`department` (`idDepartment`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 7
DEFAULT CHARACTER SET = utf8;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
