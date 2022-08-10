#include <Arduino.h>
#include <SoftwareSerial.h>
#include <stringSerial.h>
stringSerial a = stringSerial();
void setup(){
  a.init();
  Serial.begin(9600);
}
void loop(){
  //Serial.println(716.6029, 4);
  //Serial.println(a.convert10dString(716.6029));
  a.copyToSTR("TES1234556787");
  Serial.print(a.getdata(lat));
  a.clearBuff();
  Serial.print("    ");
  Serial.println(a.getdata(h2s));
  a.clearBuff();
}
/*
void transmitSensor(int i){
  if(i==0){
    Serial.print(a.getdata(humadity));
  }else if(i==1){
    Serial.print(a.getdata(temperature));
  }else if(i==2){
    Serial.print(a.getdata(altitude));
  }else if(i==3){
    Serial.print(a.getdata(roll));
  }else if(i==4){
    Serial.print(a.getdata(pitch));
  }else if(i==5){
    Serial.print(a.getdata(yaw));
  }else if(i==6){
    Serial.print(a.getdata(co));
  }else if(i==7){
    Serial.print(a.getdata(co2));
  }else if(i==8){
    Serial.print(a.getdata(nh4));
  }else if(i==9){
    Serial.println(a.getdata(h2s));
  }else if(i==10){
    Serial.print(a.getdata(battery));
  }else if(i==11){
    Serial.print(a.getdata(compass));
  }
}

void setup() {
  a.init();
  Serial.begin(9600);
}

void loop() {
  /*
  Serial.print(a.getPPM_co());
  Serial.print("//");
  Serial.println(a.getPPM_h2s());
  //Serial.print("//");
  //Serial.println(a.getYaw());
  
  Serial.print(a.convert4dString(a.getRoll()));
  Serial.print("//");
  Serial.print(a.convert4dString(a.getPitch()));
  Serial.print("//");
  Serial.println(a.convert4dString(a.getYaw()));
  
  
  static int i=0;
  transmitSensor(i);
  if(i>11){i=0;}else{i++;}
  a.clearBuff();
  
}
*/