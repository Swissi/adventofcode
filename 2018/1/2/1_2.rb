sum = 0
sums = [sum]
factors = []
gotone = false

File.open("input.txt", "r") do |f|
  f.each_line do |line|
    factors.push(Integer(line))
  end
end

loop do
  factors.each do |factor|
    sum += factor
    if sums.include?(sum)
      puts "got one!"
      gotone = true
      puts sum
      break
    else
      sums.push(sum)
    end
  end
  break if gotone == true
end
