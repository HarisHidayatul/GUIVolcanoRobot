#include <stringSerial.h>

stringSerial::stringSerial(/* args */)
{
    initHeader();
}

stringSerial::~stringSerial()
{
    initHeader();
}
void stringSerial::clearBuff(){
    int data = countString(data_char);
    for(int i =0;i<data;i++){
        data_char[i] = NULL;
    }
}
void stringSerial::copyToSTR(char *s1){
    int data = countString(s1);
    int data2 = countString(str_save);
    for (int i = 0; i < data2; i++)
    {
        str_save[i] = NULL;
    }
    
    for(int i =0;i < data;i++){
        str_save[i] = s1[i];
    }
}
char* stringSerial::getStr(){
    return str_save;
}

char* stringSerial::getdata(int codeSensor){
    initHeader();
    if((codeSensor == lat)||(codeSensor == lng)){
        appendChar(getStr());
    }else{
        data_char[1] = unitNumb;
        appendChar(convert2dString(codeSensor));
        accSensor(codeSensor);
        processFCS();
    }
    int b = countString(data_char);
    data_char[b] = delimiter;
    return (char *)data_char;
}
void stringSerial::accSensor(int codeSensor){
    switch (codeSensor)
    {
    case temperature:
        appendChar(convert4dString(getTempBME280()));
        /* code */
        break;
    case altitude:
        appendChar(convert4dString(getAltitudeBME280()));
        break;
    case humadity:
        appendChar(convert4dString(getHumadityBME280()));
        break;
    case roll:
        appendChar(convert4dString(getRoll()));
        break;
    case pitch:
        appendChar(convert4dString(getPitch()));
        break;
    case yaw:
        appendChar(convert4dString(getYaw()));
        break;
    case co:
        appendChar(convert4dString(getPPM_co()));
        break;
    case co2:
        appendChar(convert4dString(getPPM_co2()));
        break;
    case nh4:
        appendChar(convert4dString(getPPM_nh4()));
        break;
    case h2s:
        appendChar(convert4dString(getPPM_h2s()));
        break;
    case battery:
        appendChar(convert4dString(getBatt()));
        break;
    case compass:
        appendChar(convert4dString(getCompss()));
        break;
    default:
        break;
    }
}
void stringSerial::processFCS(){
    char *a = convert2dHex(getFCS(data_char));
    int b = countString(data_char);
    for(char i=0;i<2;i++){
        data_char[i+b] = a[i];//menambahkan FCS
    }
}

void stringSerial::initHeader(){
    //Tetapkan nilai data sebagai karakter null
    for(char i=0;i<countString(data_char);i++){
        data_char[i] = '\0';
    }
    data_char[0] = header;
}

void stringSerial::appendChar(char *s){
    int a = countString(data_char);
    int b = countString(s);
    for(int i=0;i<b;i++){
        data_char[i+a] = s[i];
    }
}
char stringSerial::getFCS(char *s){
    char FCS = '\0';
    int arrSize = countString(s);
    for(int i=0;i < arrSize;i++){
        if(i == 0){
            FCS = s[0];
        }else{
            FCS = FCS ^ s[i];
        }
    }
    return FCS;
}
char* stringSerial::convert2dString(int val){
    static char data_2d[3];
    data_2d[0] = (val/10)%10 + 48;
    data_2d[1] = (val % 10) + 48;
    return data_2d;
}
char* stringSerial::convert4dString(int val){
    static char data_4d[5];
    data_4d[0] = (val/1000 % 10) +48;
    data_4d[1] = (val/100 % 10) +48;
    data_4d[2] = (val/10 % 10) +48;
    data_4d[3] = (val % 10) + 48;
    return data_4d;
}
char* stringSerial::convert10dString(float val){
    static char data_6d[10];
    dtostrf(val,10,4,data_6d);
    for(int i =0;i<10;i++){
        if(data_6d[i] == ' '){
            data_6d[i] = '0';
        }
    }
    return data_6d;
    /*
    //float val = floor(10000*val2)/10000;
    data_6d[0] = ((int)val/10000 % 10) + 48;
    data_6d[1] = ((int)val/1000 % 10) + 48;
    data_6d[2] = ((int)val/100 % 10) + 48;
    data_6d[3] = ((int)val/10 % 10) + 48;
    data_6d[4] = ((int)val % 10) + 48;
    data_6d[5] = ((int)(val * 10)%10) + 48;
    data_6d[6] = ((int)(val * 100) % 10) + 48;
    data_6d[7] = ((int)(val * 1000) % 10) + 48;
    data_6d[8] = ((int)(val *10000) % 10) + 48;
    return data_6d;*/
}
char* stringSerial::convert2dHex(char a){
    static char data_2d[2];
    const char bil_hexa[17] = "0123456789ABCDEF";
    data_2d[0] = bil_hexa[(a/16 % 16)];
    data_2d[1] = bil_hexa[(a%16)];
    return data_2d;
}
int stringSerial::countString(char *s){
    for(int i=0;i<100;i++){
        if(s[i]=='\0'){
            return i;
        }
    }
    return 0;
}
int stringSerial::convertToInt(char *s){
   int conv = 0;
    for(int i = 0;i<countString(s);i++){
        int bilngn = s[i]-48;
        if((bilngn <10)&&(bilngn>=0)){
            conv = (conv*10) + bilngn;
        }else{
            return conv;
        }
    }
    return conv;
}
float stringSerial::strToFloat(char* s){
    return atof(s);
}