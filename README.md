# ScoresDB

Generates a scores.db based on your current osu! stable installation and all available replays in `Data\r`.

(should be) a drop-in replacement for your current scores.db. Remember to keep backups of both your `Data\r` (hidden folder) and scores.db if you care about them.

Use in case your database is corrupted or osu! automatically purged some plays you want after you deleted and readded a mapset.

Serves as an alternative to writing a batch script to have osu! import these replays for you. It may even be faster too.

Note that osu! will continue to purge the scores.db of any map that you do not currently have.

Information used:
- [HoLLy-HaCKeR/osu-database-reader](https://github.com/HoLLy-HaCKeR/osu-database-reader)
- [osu! wiki: osr file format](https://osu.ppy.sh/wiki/en/osu%21_File_Formats/Osr_%28file_format%29)
- [osu! wiki: database file formats](https://osu.ppy.sh/wiki/en/osu%21_File_Formats/Db_%28file_format%29#scores.db)

Not affiliated with osu! or ppy Pty Ltd. Use at your own risk.