# This python script is intended for zipping release-ready distributable files for EasyImgur

#!/usr/bin/env python
import os
import zipfile
import struct
import re


def tryParseVersionFromString(string):
	result = re.match("(?P<major>\d+)\.(?P<minor>\d+)\.(?P<patch>\d+)", string)
	if result is None:
		return None
	major = int(result.group("major"))
	minor = int(result.group("minor"))
	patch = int(result.group("patch"))
	return (major, minor, patch)


# Asks the user (with additional confirmation request) about the version of this executable and returns a tuple with (major, minor, patch) version numbers
def inputDistVersion():
	first_attempt = None
	second_attempt = None

	while True:
		first_attempt = tryParseVersionFromString(raw_input("Which version are we dealing with (major.minor.patch)? "))
		if first_attempt is not None:
			break
	
	while True:
		second_attempt = tryParseVersionFromString(raw_input("Confirm the input version (major.minor.patch): "))
		if second_attempt is not None:
			break
	
	# If both inputs are the same, return the result
	if first_attempt == second_attempt:
		return first_attempt
		
	return None
	

# Returns the path to the dist folder
def getDistPath():
	return os.path.abspath(__file__ + "../../../dist/release/")


# Zip all files in folder 'path' to zipfile handle 'ziph'. If the zipfile itself is present in the 
# target folder, it is ignored
def zipFolder(path, ziph):
	print "Zipping to file '{0}'...".format(ziph.filename)
	
	for root, dirs, files in os.walk(path):
		for file in files:
			# Don't attempt to zip the target zipfile itself if it is present in the target directory.
			if file == os.path.basename(ziph.filename):
				continue

			# Don't attempt to zip any zip files in the directory because they could be from previous releases
			if file.endswith(".zip"):
				print " ignoring: {0}".format(file)
				continue
				
			print " zipping: {0}".format(file)
			ziph.write(os.path.join(root, file), file)
			
	print "Zip finished"

	
def main():
	version = inputDistVersion()
	
	if version is None:
		print "The input version numbers do not match!"
		return
	
	zipfilename = os.path.join(getDistPath(), "EasyImgur_v{}-{}-{}.zip".format(version[0], version[1], version[2]))
	zipf = zipfile.ZipFile(zipfilename, 'w', zipfile.ZIP_DEFLATED)
	zipFolder(getDistPath(), zipf)
	zipf.close()
	
if __name__ == '__main__':
	main()