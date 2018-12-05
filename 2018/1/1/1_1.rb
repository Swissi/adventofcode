sum = 0
content = File.readlines('i.txt')
content.each do |c|
  sum = sum + Integer(c)
end
puts sum