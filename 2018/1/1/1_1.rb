sum = 0
File.open("input.txt", "r") do |f|
  f.each_line do |line|
    sum = sum + Integer(line)
  end
end
puts sum