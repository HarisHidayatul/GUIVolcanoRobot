#include <MQUnifiedsensor.h>
#include <Wire.h>
#include <SPI.h>
#include <Adafruit_Sensor.h>
#include <Arduino.h>
#include <Adafruit_BME280.h>
#include <Adafruit_BNO055.h>
#include <utility/imumaths.h>
// LSM303 Compass Library
#include <LSM303.h>

// data untuk bme sensor
#define BME_SCK 13
#define BME_MISO 12
#define BME_MOSI 11
#define BME_CS 10
#define SEALEVELPRESSURE_HPA (1013.25)


//Definitions
#define placa "Arduino NANO"
#define Voltage_Resolution 5
#define pin1 A2 //Analog input 0 of your arduino
#define type1 "MQ-135" //MQ135
#define ADC_Bit_Resolution 10 // For arduino UNO/MEGA/NANO
#define RatioMQ135CleanAir 3.6//RS / R0 = 3.6 ppm  
//#define calibration_button 13 //Pin to calibrate your sensor

#define placa "Arduino NANO"
#define pin2 A0 //Analog input 4 of your arduino
#define type2 "MQ-136" //MQ4
#define RatioMQ136CleanAir 3.6//RS / R0 = 3.6 ppm  

//Declare Sensor
//MQUnifiedsensor MQ135(placa, Voltage_Resolution, ADC_Bit_Resolution, pin1, type1);
//MQUnifiedsensor MQ136(placa,Voltage_Resolution,ADC_Bit_Resolution, pin2,type2);

class accSensor
{
private:
    /* data */
    MQUnifiedsensor co = MQUnifiedsensor(placa, Voltage_Resolution, ADC_Bit_Resolution, pin1, type1);
    MQUnifiedsensor co2 = MQUnifiedsensor(placa, Voltage_Resolution, ADC_Bit_Resolution, pin1, type1);
    MQUnifiedsensor nh4 = MQUnifiedsensor(placa, Voltage_Resolution, ADC_Bit_Resolution, pin1, type1);
    MQUnifiedsensor h2s = MQUnifiedsensor(placa,Voltage_Resolution,ADC_Bit_Resolution, pin2,type2);//mq136

    Adafruit_BME280 bme; // I2C
    Adafruit_BNO055 bno = Adafruit_BNO055(55);
    
    LSM303 cmps;

    bool status;
    
    static float lat_position;
    static float lng_position;

public:
    accSensor(/* args */);
    ~accSensor();
    int getTempBME280();
    int getAltitudeBME280();
    int getHumadityBME280();
    int getRoll();
    int getPitch();
    int getYaw();
    int getPPM_co();
    int getPPM_co2();
    int getPPM_nh4();
    int getPPM_h2s();
    int getBatt();
    int getCompss();
    float getLat();
    float getLng();
    void setLat(float val);
    void setLng(float val);
    void init();
    void initBME280();
    void initBNO055();
    void initco();
    void initco2();
    void initnh4();
    void initCompss();
    void inith2s();
};
