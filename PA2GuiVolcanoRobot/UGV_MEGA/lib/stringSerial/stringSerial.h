#include <accSensor.h>
#define maxData 20
#define altitude 0
#define humadity 1
#define co 2
#define battery 3
#define compass 4
#define temperature 5
#define h2s 6
#define speed 7
#define roll 8
#define pitch 9
#define yaw 10
#define co2 11
#define nh4 12
#define lat 13
#define lng 14

class stringSerial : public accSensor
{
private:
    /* data */
    char data_char[maxData];
    char str_save[maxData] = "";
    const char header = '#';
    const char delimiter = '%';
    const char unitNumb = '0';
    //int mode =0;
    void initHeader();
    void processFCS();
    void accSensor(int codeSensor);
    void appendChar(char *s);
    char* convert2dHex(char a);
    char* convert2dString(int val);
    int convertToInt(char *s);
    char getFCS(char *s);
public:
    stringSerial(/* args */);
    ~stringSerial();
    int countString(char *s);
    float strToFloat(char *s);
    char *getdata(int codeSensor);
    void copyToSTR(char *s1);
    char* getStr();
    void clearBuff();
    char* convert4dString(int val);
    char* convert10dString(float val);
};
