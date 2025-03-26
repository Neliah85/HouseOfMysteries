from selenium import webdriver
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.common.by import By
import time
options = webdriver.ChromeOptions()
driver = webdriver.Chrome(options=options)
driver.set_window_size(1920, 1080)

driver.get("http://localhost:3000/") 
driver.save_screenshot("HOM_fooldal.png")
time.sleep(5)
part=driver.find_element(By.LINK_TEXT, value="Belépés")
part.click()
driver.save_screenshot("Bejelentkezes.png")
user_name_input = driver.find_element(By.XPATH, '//input[@type="text"]')
password_input = driver.find_element(By.XPATH, '//input[@type="password"]')


user_name_input.send_keys('neliah69')
password_input.send_keys('Botond20221024!')
time.sleep(1)

login_button = driver.find_element(By.XPATH, '//button[text()="Belépés"]')
login_button.click()
time.sleep(1)
driver.save_screenshot('first_login.png')

time.sleep(1)
time.sleep(5)
driver.quit
