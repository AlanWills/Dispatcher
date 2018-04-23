for /r %%i in (*.mp3) do (
	ffmpeg -y -i %%~nxi -ac 2 -codec:a libmp3lame -b:a 48k -ar 16000 Output\%%~nxi
)