# AWC
Alberts Word Count or AWC is a tool made for counting all space separated words in a file. The differences from `wc` is that `awc` supports more advanced functionality, such as counting unique words or listing all words and their respective occurrences.

# Examples
Getting all unique words in a file
```bash
awc --file somefile.txt --unique-words
```
Getting all unique words in a file with their respective occurrences
```bash
awc --file somefile.txt --unique-words --word-count
```
Getting the word count of a file
```bash
awc --file somefile.txt --word-count
```
Getting the unique word count of a file
```bash
awc --file somefile.txt --unique-count
```
Using stdin
```bash
cat somefile.txt | awc --unique-count
```
Using trie implementation
```bash
awc --file somefile.txt --trie
```
Getting help
```bash
awc --help
```


