
EDA
===

KiCad 5 is required to work on this project properly. I have no experience with EDA files in a repo, and how well they merge, so it might be better to request changes in an issue or ask for exclusive access so we do not destroy each others changes ;)


Bus
---

As Ulli suggested, the Bus should be CAN bus. As the schedule is tight a backup plan exists in the schematics, which allows to Bridge the CAN driver and use I2C directly. Note, however that in that case only 1 PCB should have it's pullups populated, as they are on 1k8 which will allow high speeds at 3mA low source current.

https://cdn-reichelt.de/documents/datenblatt/A200/MCP2551_MIC.pdf


Sensor
------

Martin found a nice PCB which holds the BMP280 suggested by Erich and Bernd as well as a Gyro and more. This is called a 10DOF, and can be sourced more or less easily. Boards differ in pin count, but the first 4 pins are always:

1. VIN (wird mit 3.3V gespeist)
2. GND
3. 3V3 (out)
4. SDA
5. SCL

The other pins are ignored.
Ebay: Eckstein

https://store.invensense.com/datasheets/invensense/MPU-6050_DataSheet_V3%204.pdf
https://www.ebay.de/itm/10DOF-Lsm303d-L3gd20-BMP180-Breakout-Modul-Beschleunigungsmesser-Kompass/253091721043?hash=item3aed712f53:g:O0UAAOSw~I5ZjqvM:rk:18:pf:0
https://www.ebay.de/itm/New-SPI-IIC-MPU-9250-MS5611-High-Precision-9-Axis-10DOF-Altitude-sensor-Module/272672204397?hash=item3f7c87a66d:g:fSAAAOSwn-tZGX4T:rk:14:pf:0
https://www.ebay.de/itm/GY-87-10DOF-3-Achsen-Gyroskop-Accelerometer-Sensor-Modul-SE01006/281460524502?hash=item41885ac5d6:g:vKwAAOSwhnlb~pYd:rk:13:pf:0
https://www.reichelt.at/arduino-grove-sensor-imu-10dof-v2-0-grv-imu-10dof-v2-p243392.html?PROVID=2788&gclid=EAIaIQobChMIuNSy9rWQ3wIVhOmaCh0uWwYPEAQYAyABEgIEOfD_BwE&&r=1
https://www.banggood.com/GY-91-MPU9250-BMP280-10DOF-Acceleration-Gyroscope-Compass-Nine-Shaft-Sensor-Module-p-1129541.html?rmmds=search&cur_warehouse=CN

10DOF schematics
http://kom.aau.dk/~jdn/edu/doc/arduino/sensors/gy80gy87gy88/gy87sch.jpg


Motors
------

As there is so little time and much room for error, there is a Motor-Daughterboard connector which will eventually allow to run DC motors via L293D. It's first job is to run Servos instead and allow a Pan/Tilt movement of the Board.


Power
-----

Stand alone boards can be powered via a beefy USB, or can disconnect the solder jumper and supply external Power via X101
