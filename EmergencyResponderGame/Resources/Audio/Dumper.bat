copy NUL mylist.txt

for %%i in (*.mp3) do (
	@echo file '%%i' >> mylist.txt
)