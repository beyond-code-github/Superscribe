require 'rubygems'
require 'jekyll'
require 'tmpdir'
 
desc "Generate blog files"
task :generate do
system "Jekyll build --source . --destination D:\\Code\\Designs\\Generated"
end