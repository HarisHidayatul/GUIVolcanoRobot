#include <accSensor.h>
float accSensor::lat_position = 0;
float accSensor::lng_position = 0;
accSensor::accSensor(/* args */)
{
}

accSensor::~accSensor()
{
}
int accSensor::getPPM_co(){
    co.update();
    float z = co.readSensor();
    z = z*100;
    return z;
}
int accSensor::getBatt(){
    int a = analogRead(A1);
    return a/4.65;
}
int accSensor::getCompss(){
    cmps.read();
    float heading = cmps.heading();
    return (int)heading;
}
void accSensor::initCompss(){
    Wire.begin();
    cmps.init();
    cmps.enableDefault();
  
    /*
    Calibration values; the default values of +/-32767 for each axis
    lead to an assumed magnetometer bias of 0. Use the Calibrate example
    program to determine appropriate values for your particular unit.
    */
    cmps.m_min = (LSM303::vector<int16_t>){-32767, -32767, -32767};
    cmps.m_max = (LSM303::vector<int16_t>){+32767, +32767, +32767};
}
int accSensor::getPPM_co2(){
    co2.update();
    float z = co2.readSensor();
    z = z*100;
    return z;
}
int accSensor::getPPM_nh4(){
    nh4.update();
    float z = nh4.readSensor();
    z = z*100;
    return z;
}
int accSensor::getPPM_h2s(){
    h2s.update();
    float z = h2s.readSensor();
    z = z*100;
    return z;
}
void accSensor::initco(){
    co.setRegressionMethod(1); //_PPM =  a*ratio^b
  co.setA(605.18); co.setB(-3.937); // Configurate the ecuation values to get NH4 concentration
  co.init();
  //callibration mq135
  float calcR0 = 0;
  for(int i = 1; i<=10; i ++)
  {
    co.update(); // Update data, the arduino will be read the voltage on the analog pin
    calcR0 += co.calibrate(RatioMQ135CleanAir);
    //Serial.print(".");
  }
  co.setR0(calcR0/10);
  //if(isinf(calcR0)) {Serial.println("Open Loop MQ135"); while(1);}
  //if(calcR0 == 0){Serial.println("Close Loop MQ135"); while(1);}
  co.serialDebug(true);
}
void accSensor::initco2(){
    co2.setRegressionMethod(1); //_PPM =  a*ratio^b
  co2.setA(110.47); co2.setB(-2.862); // Configurate the ecuation values to get NH4 concentration
  co2.init();
  //callibration mq135
  float calcR0 = 0;
  for(int i = 1; i<=10; i ++)
  {
    co2.update(); // Update data, the arduino will be read the voltage on the analog pin
    calcR0 += co2.calibrate(RatioMQ135CleanAir);
    //Serial.print(".");
  }
  co2.setR0(calcR0/10);
  //if(isinf(calcR0)) {Serial.println("Open Loop MQ135"); while(1);}
  //if(calcR0 == 0){Serial.println("Close Loop MQ135"); while(1);}
  co2.serialDebug(true);
}
void accSensor::initnh4(){
    nh4.setRegressionMethod(1); //_PPM =  a*ratio^b
  nh4.setA(102.2); nh4.setB(-2.473); // Configurate the ecuation values to get NH4 concentration
  nh4.init();
  //callibration mq135
  float calcR0 = 0;
  for(int i = 1; i<=10; i ++)
  {
    nh4.update(); // Update data, the arduino will be read the voltage on the analog pin
    calcR0 += nh4.calibrate(RatioMQ135CleanAir);
    //Serial.print(".");
  }
  nh4.setR0(calcR0/10);
  //if(isinf(calcR0)) {Serial.println("Open Loop MQ135"); while(1);}
  //if(calcR0 == 0){Serial.println("Close Loop MQ135"); while(1);}
  nh4.serialDebug(true);
}
void accSensor::inith2s(){
    h2s.setRegressionMethod(1); //_PPM =  a*ratio^b
  h2s.setA(102.2); h2s.setB(-2.473); // Configurate the ecuation values to get NH4 concentration
  h2s.init();
  //callibration mq136
  float calcR1 = 0;
  for(int i = 1; i<=10; i ++)
  {
    h2s.update(); // Update data, the arduino will be read the voltage on the analog pin
    calcR1 += h2s.calibrate(RatioMQ136CleanAir);
    //Serial.print(".");
  }
  h2s.setR0(calcR1/10);
  //if(isinf(calcR1)) {Serial.println("Open Loop MQ136"); while(1);}
  //if(calcR1 == 0){Serial.println("Close Loop MQ136"); while(1);}
  h2s.serialDebug(true);
}
void accSensor::setLat(float val){
    lat_position = val;
}
void accSensor::setLng(float val){
    lng_position = val;
}
float accSensor::getLat(){
    return lat_position;
}
float accSensor::getLng(){
    return lng_position;
}
int accSensor::getRoll(){
    //return 0;
    imu::Vector<3> euler = bno.getVector(Adafruit_BNO055::VECTOR_EULER);
    int z = euler.x();
    z = z*10;
    if(z<0){
        z = z*(-1);
        z++;
    }
    return z;
}
int accSensor::getPitch(){
    //return 0;
    imu::Vector<3> euler = bno.getVector(Adafruit_BNO055::VECTOR_EULER);
    int z = euler.y();
    z = z*10;
    if(z<0){
        z = z*(-1);
        z++;
    }
    return z;
}
int accSensor::getYaw(){
    //return 0;
    imu::Vector<3> euler = bno.getVector(Adafruit_BNO055::VECTOR_EULER);
    int z = euler.z();
    z = z*10;
    if(z<0){
        z = z*(-1);
        z++;
    }
    return z;
}
void accSensor::initBNO055(){
    status = bno.begin();
    //delay(1000);
    bno.setExtCrystalUse(true);
}
int accSensor::getHumadityBME280(){
    return (int)bme.readHumidity();
}
int accSensor::getTempBME280(){
    float z = bme.readTemperature();
    z = z*10;
    return (int)z;
}
int accSensor::getAltitudeBME280(){
    return (int)bme.readAltitude(SEALEVELPRESSURE_HPA);
}
void accSensor::init(){
    initBME280();
    initBNO055();
    initco();
    initco2();
    initnh4();
    inith2s();
    initCompss();
}
void accSensor::initBME280(){
    status = bme.begin(0x76);
}