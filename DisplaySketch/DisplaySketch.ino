// sample input: <1,0,1,12,123,0.5,0,0,4,5000,20,20,10,1,1,0.5,1>

#include <SPI.h>
#include "Adafruit_GFX.h"
#include <MCUFRIEND_kbv.h>

#define BLACK   0x0000
#define BLUE    0x001F
#define RED     0xF800
#define GREEN   0x07E0
#define CYAN    0x07FF
#define MAGENTA 0xF81F
#define YELLOW  0xFFE0
#define WHITE   0xFFFF
#define GRAY    0x4228

#define SCREEN_W  320
#define SCREEN_H  240

MCUFRIEND_kbv tft;

short prevSpeed = -1;
short speed = 0;

short prevGear = -1;
short gear = 0;

int prevMinutes = -1;
int prevSeconds = -1;
int prevMilliseconds = -1;
int minutes = 0;
int seconds = 0;
int milliseconds = 0;

float prevDeltaSector1 = -1;
float prevDeltaSector2 = -1;
float prevDeltaLap = -1;
float deltaSector1 = 0;
float deltaSector2 = 0;
float deltaLap = 0;

short prevRpmLevel = -1;
short prevEngineRpm = -1;
short rpmLevel = 0;
short engineRpm = 0;

short prevTiresFL = -1;
short prevTiresFR = -1;
short prevTiresRL = -1;
short prevTiresRR = -1;
short tiresFL = 0;
short tiresFR = 0;
short tiresRL = 0;
short tiresRR = 0;

float prevErsLap = -1;
float prevErsCharge = -1;
short prevErsMode = -1;
float ersLap = 0;
float ersCharge = 0;
short ersMode = 3;

short prevDrsAllowed = -1;
short drsAllowed = 0;

const int DATA_SIZE = 512;
char receivedData[DATA_SIZE];
char packetData[DATA_SIZE]; 
boolean newDataReady = false;
char* strtokIndex; 
boolean recvInProgress = false;
int dataIndex = 0;
  
void setup() {
  uint16_t ID = tft.readID(); 
  tft.begin(ID);
  tft.setRotation(1);
  tft.fillScreen(GRAY);

  Serial.begin(115200);
  
  drawDisplay();
}

void loop() {  
  //setTestValues();
  receivePacket();

  if (newDataReady) {
    newDataReady = false;
    getValuesFromPacket();
    
    drawDisplay();  
    updatePrevValues();
    
    Serial.println("ack");
    }
}

void drawDisplay() {
  drawSpeed();
  drawGear();
  drawTime();
  drawRPM();
  drawTires();
  drawERS();
  drawDRS();
}

void getValuesFromPacket() {  
  strcpy(packetData, receivedData);

  speed = getIntFromPacket(packetData);

  gear = getIntFromPacket(NULL);

  minutes = getIntFromPacket(NULL);
  seconds = getIntFromPacket(NULL);
  milliseconds = getIntFromPacket(NULL);

  deltaSector1 = getFloatFromPacket(NULL);
  deltaSector2 = getFloatFromPacket(NULL);
  deltaLap = getFloatFromPacket(NULL);

  rpmLevel = getIntFromPacket(NULL);
  engineRpm = getIntFromPacket(NULL);

  tiresRL = getIntFromPacket(NULL);
  tiresRR = getIntFromPacket(NULL);
  tiresFL = getIntFromPacket(NULL);
  tiresFR = getIntFromPacket(NULL);
  

  ersLap = getFloatFromPacket(NULL);
  ersCharge = getFloatFromPacket(NULL);
  ersMode = getIntFromPacket(NULL);

  drsAllowed = getIntFromPacket(NULL);
}

void updatePrevValues() {  
  prevSpeed = speed;
  
  prevGear = gear;
  
  prevMinutes = minutes;
  prevSeconds = seconds;
  prevMilliseconds = milliseconds;
  
  prevDeltaSector1 = deltaSector1;
  prevDeltaSector2 = deltaSector2;
  prevDeltaLap = deltaLap;
  
  prevRpmLevel = rpmLevel;
  prevEngineRpm = engineRpm;
  
  prevTiresFL = tiresFL;
  prevTiresFR = tiresFR;
  prevTiresRL = tiresRL;
  prevTiresRR = tiresRR;

  
  prevErsLap = ersLap;
  prevErsCharge = ersCharge;
  prevErsMode = ersMode;

  prevDrsAllowed = drsAllowed;
}

void drawSpeed() {
  if (prevSpeed != speed) {
    tft.setTextColor(YELLOW, BLACK);
    tft.setCursor(50, 70);
    char buffer[10];
    snprintf(buffer, sizeof(buffer), "%03i", speed);
    tft.setTextSize(5);
    tft.print(buffer);
    tft.setTextSize(2);
    tft.println("km/h");
  }
}

void drawGear() {
  if (prevGear != gear) {
    tft.setTextColor(YELLOW, BLACK);
    tft.setCursor(280, 130);  
    tft.setTextSize(5);
  
    if (gear > 0) {
      tft.print(gear); 
    }
    else if (gear == 0) {
      tft.print("N"); 
    }
    else {
      tft.print("R"); 
    }
  }
}

void drawTime() {
  char timeStr[32];
  
  if (prevMinutes != minutes || prevSeconds != seconds || prevMilliseconds != milliseconds) {
    tft.setTextColor(YELLOW, BLACK);
    tft.setCursor(25, 15);  
    tft.setTextSize(3);    
    snprintf(timeStr, sizeof(timeStr), "%02i:%02i.%03i", minutes, seconds, milliseconds);
    tft.println(timeStr);
  }

  tft.setTextSize(2);
  
  if (prevDeltaSector1 != deltaSector1) {
    if (deltaSector1 == 0) {
      tft.setTextColor(YELLOW, BLACK); 
    }
    else if (deltaSector1 > 0) {
      tft.setTextColor(RED, BLACK); 
    }
    else {
      tft.setTextColor(GREEN, BLACK);
    }
    
    tft.setCursor(220, 5);    
    dtostrf(deltaSector1, 6, 3, timeStr);  
    tft.println(timeStr);
  }

  if (prevDeltaSector2 != deltaSector2) {
    if (deltaSector2 == 0) {
      tft.setTextColor(YELLOW, BLACK); 
    }
    else if (deltaSector2 > 0) {
      tft.setTextColor(RED, BLACK); 
    }
    else {
      tft.setTextColor(GREEN, BLACK);
    }
    
    tft.setCursor(220, 25);    
    dtostrf(deltaSector2, 6, 3, timeStr);  
    tft.println(timeStr);
  }

  if (prevDeltaLap != deltaLap) {
    if (deltaLap == 0) {
      tft.setTextColor(YELLOW, BLACK); 
    }
    else if (deltaLap > 0) {
      tft.setTextColor(RED, BLACK); 
    }
    else {
      tft.setTextColor(GREEN, BLACK);
    }
    
    tft.setCursor(220, 45);   
    dtostrf(deltaLap, 6, 3, timeStr);  
    tft.println(timeStr);
  }
}

void drawRPM() {
  if (prevRpmLevel != rpmLevel) {
    for (int x = 0; x < 10; x ++) {
      if (x >= rpmLevel) {
        tft.fillCircle(80 + 20 * x, 160, 8, BLACK);
      }
      else if (x < 5) {
        tft.fillCircle(80 + 20 * x, 160, 8, BLUE);
      }
      else { 
        tft.fillCircle(80 + 20 * x, 160, 8, CYAN);        
      }
    }
  }

  if (prevEngineRpm != engineRpm) {  
    tft.setTextColor(YELLOW, BLACK);
    tft.setCursor(160, 130);  
    tft.setTextSize(2);  
    char buffer[32];
    snprintf(buffer, sizeof(buffer), "%05i RPM", engineRpm);
    tft.print(buffer);
  }
}
  
void drawTires() {
  char buffer[8];
  
  tft.setTextSize(3);  

  if (prevTiresFL != tiresFL) {
    tft.setCursor(160, 180);  
    snprintf(buffer, sizeof(buffer), "%3i%%", tiresFL);
    tft.setTextColor(BLACK, GREEN);
    tft.println(buffer);
  }

  if (prevTiresFR != tiresFR) {
    tft.setCursor(240, 180);  
    snprintf(buffer, sizeof(buffer), "%3i%%", tiresFR);
    tft.setTextColor(BLACK, GREEN);
    tft.println(buffer);
  }

  if (prevTiresRL != tiresRL) {
    tft.setCursor(160, 210);  
    snprintf(buffer, sizeof(buffer), "%3i%%", tiresRL);
    tft.setTextColor(BLACK, GREEN);
    tft.println(buffer);
  }

  if (prevTiresRR != tiresRR) {
    tft.setCursor(240, 210);  
    snprintf(buffer, sizeof(buffer), "%3i%%", tiresRR);
    tft.setTextColor(BLACK, GREEN);
    tft.println(buffer);
  }
}
  
void drawERS() {
  if (prevErsMode != ersMode) {
    tft.setCursor(5, 200);  
    tft.setTextSize(4);  
    tft.setTextColor(YELLOW, BLACK);
    tft.println(ersMode);
  }
  
  if (prevErsLap != ersLap) {
    tft.fillRect(32, 200, 126, 20, BLACK);  
    tft.fillRect(34, 202, 122 * ersLap, 16, GREEN);  
  }

  if (prevErsCharge != ersCharge) {
    tft.fillRect(32, 220, 126, 10, BLACK);  
    tft.fillRect(34, 220, 122 * ersCharge, 8, YELLOW);
  }
}

void drawDRS() {
  if (prevDrsAllowed != drsAllowed) {
    if (drsAllowed == 1) {
      tft.setTextColor(YELLOW, BLACK);
    }
    else {
      tft.setTextColor(RED, BLACK);
    }
  
    tft.setCursor(10, 130);  
    tft.setTextSize(2);  
    tft.print("DRS");
  }
}

void receivePacket() {  
  char startMarker = '<';
  char endMarker = '>';
  char receivedByte;

  while (Serial.available() > 0 && newDataReady == false) {
    receivedByte = Serial.read();  
    
    if (recvInProgress == true) {
      if (receivedByte != endMarker) {
        receivedData[dataIndex] = receivedByte;
        dataIndex++;
        if (dataIndex >= DATA_SIZE) {
            dataIndex = DATA_SIZE - 1;
        }
      }
      else {
          receivedData[dataIndex] = '\0';          
          recvInProgress = false;
          dataIndex = 0;
          newDataReady = true;
      }
    }

    if (receivedByte == startMarker) {      
      recvInProgress = true;
      dataIndex = 0;
      newDataReady = false;
    }
  }
}

int getIntFromPacket(char *str) {
  strtokIndex = strtok(str, ",");
  return atoi(strtokIndex); 
}

float getFloatFromPacket(char *str) {
  strtokIndex = strtok(str, ",");
  return atof(strtokIndex); 
}

void setTestValues() {
  speed += 1;

  gear = -1;

  minutes = 1;
  seconds = 23;
  milliseconds += 1;

  deltaSector1 = 23.0f;
  deltaSector2 = -14.1f;
  deltaLap = 1;

  rpmLevel = 7;
  engineRpm = 10000;

  tiresFL = 0;
  tiresFR = 2;
  tiresRL = 20;
  tiresRR = 35;

  ersLap = 1;
  ersCharge = 0.75;  
  ersMode = 3;

  drsAllowed = 0;
}
